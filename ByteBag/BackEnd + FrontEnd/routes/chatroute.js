const fs = require('fs');
const express = require('express');
const route = express.Router();
const pool = require('./db');

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

route.get('/chat', checkAuth, (req, res) => {
    fs.readFile('./FrontEnd/views/marketchat/chat.html', (err, file) => {
        res.setHeader('content-type', 'text/html');
        res.end(file);
    });
})
route.get('/chat.css', (req, res) => {
    fs.readFile('./FrontEnd/Public/chat/chat.css', (err, file) => {
        res.end(file);
    });
})
route.get('/chat.js', (req, res) => {
    fs.readFile('./FrontEnd/Public/chat/chat.js', (err, file) => {
        res.end(file);
    });
})


route.get('/profilepic.png', (req, res) => {
    fs.readFile('./FrontEnd/profilepic.png', (err, file) => {
        res.end(file);
    });
})


route.get('/get-all-chat/:mode/:uid', (req, res) => {
    let mode = req.params.mode;
    let uid = req.params.uid;
    //console.log(mode);
    if (mode == "vasarlas") {
        pool.query(`
            SELECT chattomarket.chatID, chattomarket.userID, user.username AS userUsername, chattomarket.marketID, market.userID AS marketUserID, market_user.username AS marketUsername, market.title
            FROM chattomarket 
            LEFT JOIN user ON chattomarket.userID = user.userID
            LEFT JOIN market ON chattomarket.marketID = market.marketID
            LEFT JOIN user AS market_user ON market.userID = market_user.userID
            WHERE chattomarket.userID = ?
        `, [uid], function (err, results) {
            if (err) console.error(err);
            //console.log(results);
            res.send(results);
        });
    }
    if (mode == "eladas") {
        pool.query(`
            SELECT chattomarket.chatID, chattomarket.userID, user.username AS userUsername, chattomarket.marketID, market.userID AS marketUserID, market_user.username AS marketUsername, market.title
            FROM chattomarket 
            LEFT JOIN user ON chattomarket.userID = user.userID
            LEFT JOIN market ON chattomarket.marketID = market.marketID
            LEFT JOIN user AS market_user ON market.userID = market_user.userID
            WHERE market.userID = ?
        `, [uid], function (err, results) {
            if (err) console.error(err);
            //console.log(results);
            res.send(results);
        });
    }
})
route.get('/getthischat/:id', (req, res) => {
    const id = req.params.id
    const query = "SELECT * FROM marketmessage WHERE chatID = ?;"

    pool.query(query, [id], function (err, results) {
        if (err) return err;
        //console.log(results);
        res.send(results);
    });
})
route.post('/addmessage', (req, res, next) => {
    const { userID, chatID, message } = req.body;
    //console.table(req.body);
    const query = `INSERT INTO marketmessage (userID, chatID, message, sendDATE) VALUES (?, ?, ?,  current_timestamp());`;
    pool.query(query, [userID, chatID, message], (err, results) => {
        if (err) {
            //console.error(err);
            res.status(500).send();
            return;
        }
        if (results.length === 0) {
            res.status(401).send();
            return;
        }
        //console.log(results);
        res.status(200).send()
    });
});


module.exports = route;