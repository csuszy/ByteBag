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
const transporter = require('./email');


const saltRounds = 10;



const checkAuth = (req, res, next) => {
    if (req.session && req.session.user) {
        return next();
    } else {
        return res.redirect('/');
    }
};

route.get('/forgetpas', (req, res) => {
    fs.readFile('./FrontEnd/views/help/forgetpas.html', (err, file) => {
        res.setHeader('content-type', 'text/html');
        res.end(file);
    });
})
route.get('/reset-password', (req, res) => {
    fs.readFile('./FrontEnd/views/help/resetpas.html', (err, file) => {
        res.setHeader('content-type', 'text/html');
        res.end(file);
    });
})
route.get('/forgetpass.js', (req, res) => {
    fs.readFile('./FrontEnd/Public/help/forgetpass.js', (err, file) => {
        res.end(file);
    });
})


function checkUser(email) {
    return new Promise((resolve, reject) => {
        const query = `SELECT email FROM user WHERE user.email = ?;`;
        pool.query(query, [email], (err, results) => {
            if (err) {
                return reject(err);
            }
            if (results.length == 1) {
                return resolve(true);
            } else {
                return resolve(false);
            }
        });
    });
}


route.post('/sendresetcode', (req, res) => {
    const email = req.body.email;
    let kod = Math.floor((Math.random() * 900000) + 100000);
    //console.log(`Email cím: ${email}`);
    const mailOptions = {
        from: 'help@csuszydev.hu',
        to: `${email}`,
        subject: `Jelszó visszaállítási kód`,
        text: `Itt van a jelszó visszaállításhoz szükséges kód: \n ${kod}`
    };

    transporter.sendMail(mailOptions, function (error, info) {
        if (error) {
            console.log(error);
        } else {
            let expiresAt = new Date(Date.now() + 5 * 60000); // 5 perc múlva lejár
            res.cookie('reset_code', kod, { httpOnly: true, secure: false, expires: expiresAt,maxAge: 5 * 60000});
            res.cookie('reset_email', email, { httpOnly: true, secure: false, expires: expiresAt });
            res.cookie('reset_code_expires', expiresAt.getTime(), { httpOnly: true, secure: false });

            //console.log('Email sent: ' + info.response);
            //console.log('Code: ' + kod);
            res.redirect('/reset-password');
        }
    });
});

route.post('/verify-code', async (req, res) => {
    const {kod, pass, pass2} = req.body;

    let resetCode = req.cookies.reset_code;
    let resetEmail = req.cookies.reset_email;
    let expiresAt = new Date(parseInt(req.cookies.reset_code_expires));

    if (resetCode == kod && new Date() < expiresAt && pass == pass2) {
        try {
            const userExists = await checkUser(resetEmail);
            if (userExists) {
                bcrypt.hash(pass, saltRounds, function (err, hash) {
                    if (err) { console.log(err) }
                    //console.log(userID)
                    //console.log(hashpassword)


                    const query = `UPDATE user SET password = ? WHERE user.email = ?;`;
                    pool.query(query, [hash, resetEmail], (err, results) => {
                        //console.log(results);
                        if (err) {
                            //console.error(err);
                            res.status(500).send({ message: "Szerver hiba!" });
                            return;
                        }
                        if (results.length === 0) {
                            res.status(401).send({ message: "Lejárt kód vagy a jelszavak nem egyeznek!" });
                            return;
                        }
                        else{
                            res.clearCookie('reset_code');
                            res.status(200).send();
                        }
                    });
                });
            } else {
                res.status(401).send({ message: "A megadott e-mail cím nem létezik." });
            }
        } catch (error) {
            res.status(500).send({ message: "Szerver hiba!" });
        }
    } else {
        res.status(402).send({ message: "Hibás vagy lejárt kód'" });
    }
});

route.post('/checkemail', (req,res) => {
    let {email} = req.body;
    const query = `SELECT email FROM user WHERE email = ?`;
    pool.query(query, [email], (err, results) => {
        if (err) {
            console.error(err);
            res.status(500).send({ message: "Szerver hiba!" });
            return;
        }
        if (results.length == 0) {
            console.error(err);
            res.status(401).send({ message: "Hibás felhasználónév vagy jelszó" });
            return;
        }
        if (results.length == 1) {
            res.status(200).send({message: "OK"});
        }
    });
});
route.post('/updatepass', (req, res) => {
    const { userID, updatepassword } = req.body;
    let hashpassword = "";

    bcrypt.hash(updatepassword, saltRounds, function (err, hash) {
        if (err) { console.log(err) }
        else { hashpassword = hash }
        //console.log(userID)
        //console.log(hashpassword)


        const query = `UPDATE user SET password = ? WHERE user.userID = ?;`;
        pool.query(query, [hashpassword, userID], (err, results) => {
            //console.log(results);
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
});

module.exports = route;