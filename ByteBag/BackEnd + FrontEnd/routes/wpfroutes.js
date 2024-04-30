const express = require("express");
const route = express.Router();
const bcrypt = require('bcrypt');
let saltRounds = 10;
const transporter = require('./email');
const fs = require('fs').promises;
const multer = require('multer');

const bodyParser = require('body-parser');
const path = require('path');


const storage = multer.diskStorage({
    destination: (req, file, cb) => {
        cb(null, './update/');
    },
    filename: (req, file, cb) => {
        // Az eredeti fájlnév és kiterjesztés kinyerése
        const originalName = path.basename(file.originalname, path.extname(file.originalname));
        const extension = path.extname(file.originalname);

        // Kivesszük a verziószámot a kliens által küldött adatokból
        const version = req.body.version; // Alapértelmezett verziószám, ha nincs megadva
        
        // Új fájlnév verziószámmal
        const newFilename = `${originalName}-${version}${extension}`;
        
        cb(null, newFilename); // Mentjük az új fájlnévvel
    }
});
const upload = multer({ storage: storage });

const { exec } = require('child_process');


const pool = require("./db");

route.get('/onlyuser', (req, res) => {
    const query = "SELECT user.userID, user.username, user.email, user.registerDATE, user.admin FROM user WHERE (user.admin = 0)";
    pool.query(query, (err, results) => {
        if (err) {
            res.status(401).send("HIBA");
            return;
        }
        res.json(results);
        return;
    });
});


route.get('/onlyadmin', (req, res) => {
    const query = "SELECT user.userID, user.username, user.email, user.admin, user.registerDATE FROM user WHERE (user.admin = 1)";
    pool.query(query, (err, results) => {
        if (err) {
            res.status(401).send({ message: "Hibás felhasználónév vagy jelszó!1" });
            return;
        }
        res.json(results);
        return;
    });
});


route.post('/wpflogin', (req, res) => {
    const { loginusername, loginpassword } = req.body;

    const query = `SELECT userID, username, admin, password, email FROM user WHERE (username = ? OR email = ?)`;
    pool.query(query, [loginusername, loginusername], (err, results) => {
        if (err) {
            console.error(err);
            res.status(500).send({ message: "Szerver hiba!" });
            return;
        }
        if (results.length == 0) {
            console.error(err);
            res.status(200).send({ message: "Shometing when wrong!" });
            return;
        }
        bcrypt.compare(loginpassword, results[0].password, function (err, result) {
            if (result == true && results[0].username == loginusername) {
                if (results[0].admin == 1) {
                    res.status(200).send("hitelesitve"+";"+results[0].userID.toString() +";"+results[0].username.toString()+";"+results[0].email.toString());
                    
                    return;
                }
                else{
                    res.status(200).send("Nincs ehez jogod!");
                }
            }
            else{
                res.status(200).send("Hibás felhasználónév vagy jelszó!");
                return;
            }
        });
    });
});



route.route('/user/:id')//localhost:3000/user/57562615
    .get((req, res) => {
        var id = req.params.id;

        const query = "SELECT user.userID, user.username, user.email, user.registerDATE, user.admin FROM user WHERE (user.userID = ?)";
        pool.query(query, [id], (err, results) => {
            if (err) {
                console.error(err);
                res.status(500).send({ message: "Szerver hiba!" });
                return;
            }
            if (results.length == 0) {
                //console.error(err);
                res.status(401).send({ message: "Shometing when wrong!" });
                return;
            }
            res.json(results)
        });
    })

    .put((req, res) => {
        var id = req.params.id;
        const {admin, username, newpassword} = req.body;
        //console.log(newpassword != "");
        let query = "";
        if (newpassword != "") {
            query = "UPDATE user SET user.admin = ?, user.username = ?, user.password = ? WHERE user.userID = ?;";
            bcrypt.hash(newpassword, saltRounds, function (err, hash) {
                if (err) { console.log(err) }
                pool.query(query, [admin, username, hash, id], (err, results) => {
                    if (err) {
                        //console.error(err);
                        res.status(500).send({ message: "Szerver hiba!" });
                        return;
                    }
                    if (results.length == 0) {
                        //console.error(err);
                        res.status(401).send({ message: "Shometing when wrong!" });
                        return;
                    }
                    res.status(200).send({message:"Sikeres módosítás!"})
                });
            });
        } else {
            query = "UPDATE user SET user.admin = ?, user.username = ? WHERE user.userID = ?;";
            pool.query(query, [admin, username, id], (err, results) => {
                if (err) {
                    console.error(err);
                    res.status(500).send({ message: "Szerver hiba!" });
                    return;
                }
                if (results.length == 0) {
                    //console.error(err);
                    res.status(401).send({ message: "Shometing when wrong!" });
                    return;
                }
                res.status(200).send({message:"Sikeres módosítás!"})
            });
        }
    })

    .delete((req, res) => {
        var id = req.params.id;
        const query1 = "SELECT user.email FROM user WHERE user.userID = ?;";
        pool.query(query1, [id], (err, results) => {
            if (err) {
                console.error(err);
                res.status(500).send({ message: "Szerver hiba!" });
                return;
            }
            if (results.length == 0) {
                //console.error(err);
                res.status(401).send({ message: "Shometing when wrong!" });
                return;
            }

            const query = "DELETE FROM user WHERE user.userID = ?";
            pool.query(query, [id], (err, result) => {
                if (err) {
                    console.error(err);
                    res.status(500).send({ message: "Szerver hiba!" });
                    return;
                }
                if (result.length == 0) {
                    //console.error(err);
                    res.status(401).send({ message: "Shometing when wrong!" });
                    return;
                }


                if (results[0].email == null || results[0].email == undefined || results[0].email == "") {
                    res.status(200).send({ message: "Felhasználó sikeresen törölve, de az e-mail nem lett elküldve! (Üres email mező)" });
                }
                else{
                    const mailOptions = {
                        from: 'help@csuszydev.hu',
                        to: `${results[0].email}`,
                        subject: `ByteBag - Értesítés`,
                        text: `Fiókját az adminok törölték!`
                    };
                    transporter.sendMail(mailOptions, function (error, info) {
                        if (error) {
                            console.log(error);
                            res.status(200).send({ message: "Felhasználó sikeresen törölve, de az e-mail nem lett elküldve!" });
                        } else {
                            res.status(200).send({message:"Sikeres törlés, és e-mail küldés!"})
                        }
                    });
                }
            });
        });
    })






    route.delete("/wpfdelpost/:posztID", (req, res, next) => {
        const postID = req.params.posztID;
      
        const query = `DELETE FROM forum WHERE forum.posztID = ?;`;
        pool.query(query, [postID], (err, results) => {
          //console.log(results);
          if (err) {
            //console.error(err);
            res.status(500).send();
            return;
          }
          if (results.length === 0) {
            res.status(401).send();
            return;
          }
          res.status(200).send();
        });
      });
      
      route.post("/wpfupdatepost", (req, res, next) => {
        const { editposztID, newtitle, newpost } = req.body;

        const query = `UPDATE forum SET title = ?, post = ? WHERE forum.posztID = ?;`;
          pool.query(query, [newtitle, newpost, editposztID], (err, results) => {
            if (err) {
              //console.error(err);
              res.status(500).send();
              return;
            }
            if (results.length === 0) {
              res.status(401).send();
              return;
            }
            res.status(200).send();
          });
      });




//maket edit asnd delete



route.post('/wpfupdatemarketpost', (req, res, next) => {
    const { editposztID, newtitle, newpost, editprice} = req.body;

    const query = `UPDATE market SET title = ?, marketpost = ?, price = ? WHERE market.marketID = ?;`;
        pool.query(query, [newtitle, newpost, editprice, editposztID], (err, results) => {
            if (err) {
                console.error(err);
                res.status(500).send();
                return;
            }
            if (results.length === 0) {
                res.status(401).send();
                return;
            }
            res.status(200).send()
        });
    }
);


async function deleteFile(filePath) {
    try {
        await fs.unlink(`FrontEnd/upload/${filePath}`);
        //console.log(`File ${filePath} has been deleted.`);
    } catch (err) {
        console.error(err);
    }
}


route.delete('/wpfdelmarket/:posztID', (req, res) => {
    const postID = req.params.posztID;
    //console.log(postID);
    //console.log("asd");

    //Képek törlése
    const delpicquery = 'SELECT path FROM marketpicture WHERE marketpicture.marketID = ?';
    pool.query(delpicquery, [postID], (err, ress) => {
        if (err) {
            console.log(err);
        }
        if (ress.length != 0) {
            ress.forEach(kepek => {
                deleteFile(kepek.path)
                console.log(kepek.path);
            });
        }
    })

    //hirdetés törlése
    const query = `DELETE FROM market WHERE market.marketID = ?;`;
    pool.query(query, [postID], (err, results) => {
        //console.log(results);
        if (err) {
            console.error(err);
            res.status(500).send();
            return;
        }
        if (results.length === 0) {
            res.status(401).send();
            return;
        }
        res.status(200).send();
        
    });
});



route.get('/getmsiVersion', (req, res) => {

    const msiFilePath = './download/ByteBag.msi';
    //const command = `msiinfo suminfo ${msiFilePath}`;
    const command = `msiinfo export ${msiFilePath} Property | grep "ProductCode"`;

    exec(command, (error, stdout, stderr) => {
        if (error) {
          console.error(`Error executing command: ${error.message}`);
          return;
        }
      
        if (stderr) {
          console.error(`Standard error: ${stderr}`);
          return;
        }
      
        res.send(stdout)
        //console.log(`MSI info:\n${stdout}`);
      });
})


route.get('/update/latest', (req, res) => {
    pool.query("SELECT * FROM desktopversion ORDER BY id DESC LIMIT 1; ",  (err, results) => {
        //console.log(results);
        if (err) {
            console.error(err);
            res.status(500).send();
            return;
        }
        if (results.length === 0) {
            res.status(401).send();
            return;
        }
        res.status(200).json(results);
        
    });
});
route.get('/update/all', (req, res) => {
    pool.query("SELECT * FROM desktopversion ORDER BY id DESC; ",  (err, results) => {
        //console.log(results);
        if (err) {
            console.error(err);
            res.status(500).send();
            return;
        }
        if (results.length === 0) {
            res.status(401).send();
            return;
        }
        res.status(200).json(results);
        
    });
});


route.post('/uploadNewVersion', upload.single('file'), (req, res) => {
    // Kivesszük a verziószámot a body-ból
    const version = req.body.version;
    const releaseNotes = req.body.releaseNotes;

    // Fájl adatainak mentése az adatbázisba
    const filename = req.file.filename;
    const query = "INSERT INTO desktopversion (id, version, downloadUrl, releaseNotes) VALUES (NULL, ?, ?, ?)";
    
    pool.query(query, [version, filename, releaseNotes], (err, results) => {
        if (err) {
            //console.error('Adatbázis mentési hiba:', err);
            res.status(500).json({ message: 'Hiba történt az adatbázisban' });
        } else {
            res.json({
                message: 'Fájl sikeresen feltöltve és mentve',
                filename: filename,
                path: `/update/${filename}`,
                version: version,
            });
        }
    });
});



module.exports = route;