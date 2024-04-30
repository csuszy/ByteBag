const fs = require('fs');
const fs2 = require('fs').promises;
const path = require('path');
const express = require('express');
const multer = require('multer');
const route = express.Router();
const pool = require('./db');


const storage = multer.diskStorage({
    destination: (req, file, cb) => {
        cb(null, './FrontEnd/upload/');
    },
    filename: (req, file, cb) => {
        cb(null, Date.now() + path.extname(file.originalname));
    }
});
const upload = multer({ storage: storage });


const checkAuth = (req, res, next) => {
    try {
        if (req.session && req.session.user || req.session && req.session.passport.user) {
            return next();
        } else {
            return res.redirect('/');
        }
    } catch (error) {
        return res.redirect('/');
    }
};



route.get('/market', checkAuth, (req, res) => {
    fs.readFile('./FrontEnd/views/market/market.html', (err, file) => {
        res.setHeader('content-type', 'text/html');
        res.end(file);
    });
})
route.get('/market.css', (req, res) => {
    fs.readFile('./FrontEnd/Public/market/market.css', (err, file) => {
        res.end(file);
    });
})
route.get('/market.js', (req, res) => {
    fs.readFile('./FrontEnd/Public/market/market.js', (err, file) => {
        res.end(file);
    });
})
route.get('/marketpost.css', (req, res) => {
    fs.readFile('./FrontEnd/Public/marketpost/marketpost.css', (err, file) => {
        res.end(file);
    });
})
route.get('/marketpost.js', (req, res) => {
    fs.readFile('./FrontEnd/Public/marketpost/marketpost.js', (err, file) => {
        res.end(file);
    });
})
route.post('/upload', upload.array('images'), (req, res, next) => {
    if (!req.files || req.files.length === 0) {
        return res.status(400).send('Nincs fájl feltöltve');
    }
    let images = req.files;
    const { title, post, price } = req.body;
    var userID;
    try {
        userID = req.session.user.userID;
    } catch (error) {
        userID = req.session.passport.user.userID;
    }
    const query = `INSERT INTO market (marketID, userID, title, marketpost, price, marketDATE) VALUES (NULL, "${userID}", "${title}", "${post}", ${price}, current_timestamp());`;
    pool.query(query, (err, results) => {
        if (err) {
            console.error(err);
            res.status(500).send();
            return;
        }
        if (results.length === 0) {
            res.status(401).send();
            return;
        }
        images.forEach(image => {
            const query2 = `INSERT INTO marketpicture (imgID, marketID, path) VALUES (NULL, ?, ?);`;
            pool.query(query2, [results.insertId, image.filename], (err, results) => {
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
        });
    });

    res.status(200);
    res.redirect('/market');
});
route.get('/get-market-ad', upload.array('images'), (req, res, next) => {
    const query = `
    SELECT m.marketID, m.userID, u.username, m.marketpost, m.title, m.price, m.marketDATE, mp.path
    FROM market m
    LEFT JOIN user u ON m.userID = u.userID
    LEFT JOIN (
        SELECT path, marketID
        FROM (
            SELECT path, marketID, 
            ROW_NUMBER() OVER (PARTITION BY marketID ORDER BY path) AS rn
            FROM marketpicture
        ) tmp
        WHERE rn = 1
    ) mp ON m.marketID = mp.marketID
    ORDER BY m.marketDATE DESC;
    `;
    pool.query(query, (err, results) => {
        if (err) {
            //console.error(err);
            res.status(500).send({ message: "Szerver hiba!" });
            return;
        }
        if (results.length === 0) {
            res.status(200).send({ message: "Hibás felhasználónév vagy jelszó!" });
            return;
        }
        if (results.length > 0) {
            res.setHeader('content-type', 'application/json');
            //console.table(results);
            res.json(results);
        }
        else {
            res.status(401).send();
        }
    });
});
function getPostById(postId, callback) {
    pool.query(`
    SELECT market.marketID, market.userID, user.userID, user.username, market.title, user.userID, market.marketpost, market.price, market.marketDATE 
    FROM market 
    LEFT JOIN user ON market.userID = user.userID WHERE market.marketID = ?;`,
        [postId], function (err, results) {
            if (err) return callback(err);
            callback(null, results[0]);
        });
}

route.get('/marketpost/:postID', checkAuth, (req, res, next) => {
    const postId = req.params.postID;
    var curuserID;
    try {
        curuserID = req.session.user.userID;
    } catch (error) {
        curuserID = req.session.passport.user.userID;
    }
    var thischatid = null;

    var query = 'SELECT chatID,userID, marketID FROM chattomarket WHERE userID = ? AND marketID = ?';
    pool.query(query, [curuserID, postId], function (err, ress) {
        if (err) console.error(err);
        if (ress.length == 0) {
            getPostById(postId, (err, post) => {
                if (err) return next(err);
                if (!post) return res.redirect('/404');
                if (post.userID == curuserID) {
                    res.render('markpost', { 
                        post: post,
                        thisChat: null
                    });
                }
                else{
                        res.render('markpost', { 
                            post: post,
                            thisChat: "0"
                        });
                }
            });
            if (ress.userID == curuserID) {
            }
            else{
                
            }
        }
        if (ress.length >= 1) {
            thischatid = ress[0].chatID;
            getPostById(postId, (err, post) => {
                //console.log(post.userID == curuserID);
                if (err) return next(err);
                if (!post) return res.redirect('/404');
                if (post.userID == curuserID) {
                    res.render('markpost', { 
                        post: post,
                        thisChat: null
                    });
                }
                else{
                    res.render('markpost', { 
                        post: post,
                        thisChat: thischatid
                    });
                }
            });
        }
    })
});

route.post('/create-chat', (req, res) => {
    const {logineduserID, currentPosztID} = req.body

    var query = 'INSERT INTO chattomarket (chatID, userID, marketID) VALUES (NULL, ?, ?);';
    pool.query(query, [logineduserID, currentPosztID], function (err, result) {
        if (err) {
            console.log(err);
            res.status(400).send();
            return;
        }
        else{
            res.status(200).send();
        }
    })
});

route.get('/getpic/:postID', checkAuth, (req, res, next) => {
    const postId = req.params.postID;

    pool.query(`SELECT * FROM marketpicture WHERE marketpicture.marketID = ?;`, [postId], function (err, results) {
        if (err) return err;
        //console.log(results);
        res.send(results);
    });
});
route.post('/updatemarketpost', checkAuth, (req, res, next) => {
    const { userPosztID, editposztID, newtitle, newpost, editprice} = req.body;

    if (req.session.user.userID == userPosztID || req.session.user.admin == 1) {
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
    else {
        res.status(401).send();
    }
});
async function deleteFile(filePath) {
    try {
        await fs2.unlink(`FrontEnd/upload/${filePath}`);
        //console.log(`File ${filePath} has been deleted.`);
    } catch (err) {
        console.error(err);
    }
}
route.get('/delmarket/:posztID/:curuserID', checkAuth, (req, res, next) => {
    const postID = req.params.posztID;
    const senduserID = req.params.curuserID;

    if (req.session.user.userID == senduserID || req.session.user.admin == 1) {
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
    }
    else {
        res.status(401).send();
    }
});
route.get('/download/desktopapp', (req, res) => {
    const path = `./download/ByteBag.msi`;

    // Ellenőrizze, hogy a fájl létezik-e
    fs.exists(path, (exists) => {
        if (!exists) {
            return res.status(404).send('A fájl nem létezik.');
        }

      // Beállítja a fájl letöltésének fejlécét
        res.setHeader('Content-Disposition', `attachment; filename="ByteBag.msi"`);

      // Olvassa be a fájlt, és küldje el a válaszban
        fs.readFile(path, (err, data) => {
            if (err) {
                return res.status(500).send('Hiba a fájl leolvasása közben.');
            }

            res.send(data);
        });
    });
});


module.exports = route;