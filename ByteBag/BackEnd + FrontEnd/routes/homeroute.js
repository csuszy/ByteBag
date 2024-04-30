const fs = require('fs');
const path = require('path');
const express = require('express');
const session = require('express-session');
const cookieParser = require('cookie-parser');
const mysql = require('mysql2');
const bcrypt = require('bcrypt');
const multer = require('multer');
const { createTransport } = require('nodemailer');
const https = require('https');
const route = express.Router();
const pool = require('./db');

const saltRounds = 10;
const transporter = createTransport({
    host: "smtp.rackhost.hu",
    port: 465,
    secure: true,
    auth: {
        user: "help@csuszydev.hu",
        pass: "csasziB2003",
    },
});

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


route.get('/home', checkAuth, (req, res) => {
    fs.readFile('./FrontEnd/views/home/home.html', (err, file) => {
        if (err) {
            //console.error("Fájl olvasási hiba:", err);
            res.status(500).send("Szerver hiba");
            return;
        }
        res.setHeader('content-type', 'text/html');
        res.end(file);
        return;
    });
});
route.get('/home.css', (req, res) => {
    fs.readFile('./FrontEnd/Public/home/home.css', (err, file) => {
        res.end(file);
    });
})
route.get('/home.js', (req, res) => {
    fs.readFile('./FrontEnd/Public/home/home.js', (err, file) => {
        res.end(file);
    });
})
route.get('/getpost', (reg, res) => {
    const query = `
    SELECT forum.posztID, forum.userID, user.username, forum.title, forum.post, forum.hashtag, forum.postDATE
    FROM forum
	LEFT JOIN user ON forum.userID = user.userID
    ORDER BY forum.postDATE DESC;
    `;
    pool.query(query, (err, results) => {
        if (err) {
            //console.error(err);
            res.status(500).send({ message: "Szerver hiba!" });
            return;
        }
        if (results.length === 0) {
            res.status(401).send({ message: "Hibás felhasználónév vagy jelszó!" });
            return;
        }
        if (results.length > 0) {
            res.setHeader('content-type', 'application/json');
            res.json(results);
        }
        else {
            res.status(401).send();
        }
    });
});
//ujpost
route.post('/newpost', (req, res) => {
    const { userID, newtitle, newpost } = req.body;
    const query = `INSERT INTO forum (posztID, userID, title, post, hashtag, postDATE) VALUES (NULL, ?, ?, ?, NULL, current_timestamp());`;
    pool.query(query, [userID, newtitle, newpost], (err, results) => {
        if (err) {
            //console.error(err);
            res.status(500).send({ message: "Szerver hiba!" });
            return;
        }
        if (results.length === 0) {
            res.status(401).send({ message: "Hibás felhasználónév vagy jelszó!" });
            return;
        }
        res.status(200).send({ message: "OK" });
    });
});


module.exports = route;