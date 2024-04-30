const fs = require('fs');
const path = require('path');
const express = require('express');
const jwt = require('jsonwebtoken');
const session = require('express-session');
const cookieParser = require('cookie-parser');
const mysql = require('mysql2');
const bcrypt = require('bcrypt');
const multer = require('multer');
const { createTransport } = require('nodemailer');
const https = require('https');
const route = express.Router();
const pool = require('./db');
const { ifError } = require('assert');
const saltRounds = 10;


function checkCookie(getid, getuse, getpwr) {
    const id = getid;
    const use = getuse
    const pwr = getpwr

    const maxAge = use ? use.maxAge : undefined;
    //console.table(use);
    if (id && use && pwr) {
        return true
    }
    else{return false}
}

route.get('/', (req, res) => {
    const id = req.cookies.id;
    const use = req.cookies.use;
    const pwr = req.cookies.pwr;
    if (checkCookie(id, use, pwr) == true) {
        try {
            res.redirect("/temp");
        } catch (error) {
            
        }
    }
    else
    {
        fs.readFile('./FrontEnd/views/login/login.html', (err, file) => {
            res.setHeader('content-type', 'text/html');
            res.end(file);
        });
    }
})

route.get('/temp', (req, res) => {
    fs.readFile('./FrontEnd/views/temp.html', (err, file) => {
        res.setHeader('content-type', 'text/html');
        res.end(file);
    });
})

route.get('/login.css', (req, res) => {
    fs.readFile('./FrontEnd/Public/login/login.css', (err, file) => {
        res.end(file);
    });
})
route.get('/login.js', (req, res) => {
    fs.readFile('./FrontEnd/Public/login/login.js', (err, file) => {
        res.end(file);
    });
})
route.get('/temp.js', (req, res) => {
    fs.readFile('./FrontEnd/Public/temp.js', (err, file) => {
        res.end(file);
    });
})



route.post('/cookie-login', (req, res, err) => {
    const id = req.cookies.id;
    const use = req.cookies.use;
    const pwr = req.cookies.pwr;

    if(err){ console.log(err);}

    const query = 'SELECT userID, username, email, registerDATE, admin, password FROM user WHERE (userID = ?)';
    pool.query(query, [id], (err, results) => {
        //console.log(results);
        if (err) {
            //console.error(err);
            res.status(300).send({ message: "Szerver hiba!" });
            return;
        }
        if (results.length == 0) {
            //console.error(err);
            res.status(300).send({ message: "Hibás felhasználónév vagy jelszó" });
            return;
        }
        bcrypt.compare( results[0].username, use, function (err, result) {
            //console.log(use);
            //console.log(result);
            if (result == true && results[0].userID == id) {
                //console.log("sikeres azonositas");
                req.session.user = {
                    userID: results[0].userID,
                    username: results[0].username,
                    email: results[0].email,
                    registerDATE: results[0].registerDATE,
                    admin: results[0].admin,
                };
                
                res.redirect('/home');
                res.status(200);
            }
            else if(err) {
                console.log(err);
                res.status(300);
            }
            else{
                res.status(300);
            }
        });
        if(err) {
            console.log(err);
        }
        else{
            res.status(300);
        }
    })
    if (err) {
        console.log(err);
    }
    else{
        res.status(300);
    }
})



//bejelentkezés
route.post('/login', (req, res) => {
    const { loginusername, loginpassword } = req.body;
    const rememberme = req.body.rememberme;

    
    //console.log(rememberme);
    //console.log(rememberme + "emlekezzram");


    const query = `SELECT userID, username, email, registerDATE, admin, password FROM user WHERE (username = ? OR email = ?)`;
    pool.query(query, [loginusername, loginusername], (err, results) => {
        if (err) {
            console.error(`0: ${err}`);
            res.status(500).send({ message: "Szerver hiba!" });
            return;
        }
        if (results.length == 0) {
            //console.error(`hosz: ${err}`);
            res.status(401).send({ message: "Hibás felhasználónév vagy jelszó4" });
            return;
        }
        bcrypt.compare(loginpassword, results[0].password, function (err, result) {
            if (result == true && results[0].username == loginusername) {
                req.session.user = {
                    userID: results[0].userID,
                    username: results[0].username,
                    email: results[0].email,
                    registerDATE: results[0].registerDATE,
                    admin: results[0].admin,
                };

                if (rememberme == true) {
                    bcrypt.hash(loginusername, saltRounds,function (err, hash) {
                        res.cookie('id', results[0].userID, { httpOnly: true , secure: false, maxAge: 24 * 60 * 60 * 1000});//60 perces lejarat --- ID
                        res.cookie('use', hash, { httpOnly: true , secure: false, maxAge: 24 * 60 * 60 * 1000});//60 perces lejarat --- Felhasználoév hashelve
                        res.cookie('pwr', results[0].password, { httpOnly: true, maxAge: 24 * 60 * 60 * 1000});//60 perces lejarat --- jelszo hashelve

                        
                        console.log(`\nBejelentkezett: ${loginusername} \nEkkor: ${new Date()}\n`);
                        res.redirect('/home');
                    })
                }
                else{
                    console.log(`\nBejelentkezett: ${loginusername} \nEkkor: ${new Date()}\n`);
                    res.status(200).send({message: "sikeres auth"});
                }
                
                
            }
            else{
                //console.error(`1: ${err}`);
                res.status(401).send({ message: "Hibás felhasználónév vagy jelszó!1" });
                return;
            }
            if (result == false) {
                //console.error(`2: ${err}`);
                res.status(401).send({ message: "Hibás felhasználónév vagy jelszó!2" });
                return;
            }
            if (err) {
                //console.error(`3: ${err}`);
                res.status(401).send({ message: "Hibás felhasználónév vagy jelszó!3" });
                return;
            }
        });
    });
});




//regisztrácio
route.post('/register', (req, res) => {
    const { regusername, regemail, regpassword } = req.body;
    var hashpassword = "";
    bcrypt.hash(regpassword, saltRounds, function (err, hash) {
        if (err) { console.log(err) }
        else { hashpassword = hash }
        //console.log(hash)
        //console.log(hashpassword)
        const querysafe = `SELECT username, email FROM user WHERE username = ? OR email = ?`;
        pool.query(querysafe, [regusername, regemail], (err, results) => {
            if (err) {
                //console.error(err);
                res.status(500).send({ message: "Szerver hiba!" });
                return;
            }
            if (results.length >= 1) {
                res.status(401).send({ message: "Foglalt felhasználónév vagy email!" });
                return;
            }
            else {
                var query = `INSERT INTO user (userID, username, email, password, profilPic, admin, registerDATE) VALUES (NULL, ?, ?, ?, '', 0, current_timestamp());`;
                
                
                pool.query(query, [regusername, regemail, hashpassword], (err, results) => {
                    //console.log(regusername, regemail, hashpassword);
                    if (err) {
                        //console.error(err);
                        res.status(500).send({ message: "Szerver hiba!" });
                        return;
                    }
                    if (results.length === 0) {
                        res.status(401).send({ message: "Hibás felhasználónév vagy jelszó!" });
                        return;
                    }
                    res.redirect('/');
                });
            };
        });
    });
});




module.exports = route;