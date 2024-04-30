const fs = require('fs');
const fs2 = require('fs').promises;
const path = require('path');
const express = require('express');
const session = require('express-session');
const cookieParser = require('cookie-parser');
const mysql = require('mysql2');
const bcrypt = require('bcrypt');
const multer = require('multer');
const https = require('https');
const http = require('http');

const cors = require('cors');
//github auth
const passport = require('passport');
const GitHubStrategy = require('passport-github').Strategy;

const db = require('./routes/db');

const loginRoute =  require('./routes/loginroute');
const misspassRoute =  require('./routes/misspassRoute');
const homeroute =  require('./routes/homeroute');
const postroute =  require('./routes/postroute');
const marketroute =  require('./routes/marketroute');
const wpfroutes =  require('./routes/wpfroutes');
const chatroute =  require('./routes/chatroute');

const app = express();



app.use(cookieParser());
app.use(express.json({ limit: '10mb' }));
app.use(express.urlencoded({ extended: true }));
app.use('/images', express.static('./FrontEnd/upload'));
app.use('/update', express.static('./update'));

app.use('/slider', express.static('./FrontEnd/src/image'));
app.set('view engine', 'ejs');
app.set('views', path.join(__dirname, './FrontEnd/views/res'));

app.use(session({
    secret: 'mySecretKey',
    maxAge: 24 * 60 * 60 * 1000, // 24 hours
    expires: 24 * 60 * 60 * 1000, // 24 hours
    resave: false,
    saveUninitialized: true,
    cookie: { secure: false }  // secure: true csak HTTPS alatt működik
}));

app.use(passport.initialize());
app.use(passport.session());


const allowedOrigins = ['https://bytebag.hu', /^https:\/\/cdn\.jsdelivr\.net\/npm/];

const corsOptions = {
  origin: (origin, callback) => {
    if (!origin) {
      // Ha az origin hiányzik (pl. helyi tesztelés), engedélyezd
      callback(null, true);
    } else {
      // Ellenőrizd, hogy az origin az engedélyezett eredetek között van-e
      const isAllowed = allowedOrigins.some(pattern => 
        typeof pattern === 'string' ? pattern === origin : pattern.test(origin)
      );

      if (isAllowed) {
        callback(null, true);
      } else {
        callback(new Error('Nem engedélyezett hozzáférés'));
      }
    }
  },
};

//app.use(cors(corsOptions));



// passport.use(new GitHubStrategy({
//     clientID: '',
//     clientSecret: '',
//     callbackURL: "https://bytebag.hu/auth/github/callback",
//     scope: ['user:email']
//     },
//     function(accessToken, refreshToken, profile, done) {
//         // Ellenőrizd, hogy a GitHub felhasználó már szerepel-e a táblában
//         db.query('SELECT * FROM user WHERE githubID = ?', [profile.id], (err, results) => {
//             if (err) {
//                 console.error('Error querying database:', err);
//             }
//             else {
//                 if (results.length > 0) {
//                   // A GitHub felhasználó már szerepel a táblában, visszaadjuk a meglévő felhasználót
//                     return done(null, results[0]);
//                 } 
//                 else {
//                     // A GitHub felhasználó még nincs a táblában, hozzáadjuk
//                     const user = {
//                         userID: null,
//                         githubID: profile.id,
//                         username: profile.username,
//                         email: (profile.emails && profile.emails[0]) ? profile.emails[0].value : null,
//                         registerDATE: new Date().toISOString().slice(0, 19).replace('T', ' '),
//                         admin: 0,
//                     };
//                     // Tárold el az adatokat az adatbázisban
//                     db.query('INSERT INTO user SET ?', user, (err, insertResults) => {
//                         if (err) {
//                             console.error('Error inserting user into database:', err);
//                         } else {
//                             //console.log('User inserted into database');
//                             user.userID = insertResults.insertId;
//                             return done(null, user);
//                         }
//                     });
//                 }
//             }
//         });
//     }
// ));
passport.serializeUser(function(user, done) {
    //console.table(user);
    done(null, user);
});

passport.deserializeUser(function(user, done) {
    db.query('SELECT * FROM user WHERE githubID = ?', [user.githubID], (err, results) => {
        //console.table(`97: ${results}`);
        if (err) {
            done(err, null);
        } else {
            if (results.length > 0) {
                // Ha találunk felhasználót, visszaadjuk
                done(null, results[0]);
            } else {
                // Ha nincs találat, hibát jelzünk
                done(new Error('User not found'), null);
            }
        }
    });
});



const storage = multer.diskStorage({
    destination: (req, file, cb) => {
        cb(null, './FrontEnd/upload/');
    },
    filename: (req, file, cb) => {
        cb(null, Date.now() + path.extname(file.originalname));
    }
});
const upload = multer({ storage: storage });


//session létherozása

//Logó elérése
app.get('/logo', (req, res) => {
    fs.readFile('./FrontEnd/logoR.png', (err, file) => {
        res.end(file);
    });
})



//Login ellenörzése
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

//session adatok lekérése
app.get('/get-session-data', (req, res) => {
    if (req.session && req.session.user) {
        res.json({ "user": req.session.user });
    }
    else if (req.session.passport && req.session.passport.user) {
        //console.log('asdasds');
        req.session.user = req.session.passport.user;
        res.json({ "user": req.session.user });
    } 
    else {
        res.json({ user: null });
    }
});




// GitHub bejelentkezési útvonal
app.get('/auth/github',
    passport.authenticate('github')
);

// GitHub visszahívási útvonal
app.get('/auth/github/callback',
    passport.authenticate('github', {
        failureRedirect: '/',
        failureFlash: true
    }),
    function(req, res) {
        //console.log('GitHub authentication successful. Redirecting to /home');
        res.redirect('/home');
    }
);


//Kijelentkezes
app.get('/logout', (req, res) => {
    req.session.destroy((err) => {
        if (err) {
            return res.redirect('/');
        }
        res.clearCookie('connect.sid');
        res.clearCookie('id');
        res.clearCookie('use');
        res.clearCookie('pwr');
        res.status = 200;
        res.redirect('/');
    });
});



// Pass reset routok
app.get('/forgetpas', misspassRoute)
app.get('/reset-password', misspassRoute)
app.get('/forgetpass.js', misspassRoute)
app.post('/sendresetcode', misspassRoute);
app.post('/verify-code', misspassRoute);
app.post('/checkemail', misspassRoute);
app.post('/updatepass', misspassRoute);

//Login oldal defaultkent
app.get('/', loginRoute);
app.get('/login.css', loginRoute);
app.get('/login.js', loginRoute);
app.post('/login', loginRoute);
app.post('/register', loginRoute);
app.post('/cookie-login', loginRoute);
app.get('/temp', loginRoute);
app.get('/temp.js', loginRoute);

//Home routeok
app.get('/home', checkAuth, homeroute);
app.get('/home.css', homeroute);
app.get('/home.js', homeroute);

//összes posz posztok
app.post('/newpost', homeroute);
app.get('/getpost', homeroute);

//Dinamikusan generált poszt
app.get('/post.css', postroute)
app.get('/post.js', postroute)
app.get('/post/:postID', checkAuth, postroute);
app.get('/delpost/:posztID/:curuserID', checkAuth, postroute);
app.post('/updatepost', checkAuth, postroute);
app.post('/sendcomment', postroute);
app.get('/getcomment/:currentposztID', postroute);



app.get('/search', checkAuth, (req, res) => {
    let searchTerm = req.query.q.trim()

    let sql = `SELECT forum.posztID, user.username, forum.title, forum.post FROM forum JOIN user ON forum.userID = user.userID WHERE forum.title LIKE ? OR forum.post LIKE ? OR user.username LIKE ?`;

    let query = `%${searchTerm}%`;

    db.query(sql, [query, query, query], (err, results) => {
        if (err) throw err;
        res.render('results', { results: results, searchKeyword: searchTerm });
    });
});

//PIACTÉR
app.get('/market', marketroute)
app.get('/market.css', marketroute)
app.get('/market.js', marketroute)
app.get('/marketpost.css', marketroute)
app.get('/marketpost.js', marketroute)
app.post('/upload', marketroute);
app.get('/get-market-ad', marketroute);

app.post('/create-chat', marketroute);



//wpf routeok
app.get('/onlyuser', wpfroutes);
app.get('/onlyadmin', wpfroutes);
app.post('/wpflogin', wpfroutes);
app.post('/editUser', wpfroutes);
app.delete('/wpfdelpost/:posztID', wpfroutes);
app.post('/wpfupdatepost', wpfroutes);

app.delete('/wpfdelmarket/:posztID', wpfroutes);
app.post('/wpfupdatemarketpost', wpfroutes);


app.route('/user/:id')
.get(wpfroutes)
.post(wpfroutes)
.put(wpfroutes)
.delete(wpfroutes)

app.get('/getmsiVersion', wpfroutes);


app.get('/update/latest', wpfroutes);
app.get('/update/all', wpfroutes);
app.post('/uploadNewVersion', wpfroutes);
app.get('/update', checkAuth, (req, res) => {
    if (req.session.user.admin == 1) {
        fs.readFile('./FrontEnd/views/update/update.html', (err, file) => {
            if (err) {
                //console.error("Fájl olvasási hiba:", err);
                res.status(500).send("Szerver hiba");
                return;
            }
            res.setHeader('content-type', 'text/html');
            res.end(file);
            return;
        });
    }
    else {
        res.redirect('/home');
    }
});
app.get('/update.css', (req, res) => {
    fs.readFile('./FrontEnd/Public/update/update.css', (err, file) => {
        res.end(file);
    });
})
app.get('/update.js', (req, res) => {
    fs.readFile('./FrontEnd/Public/update/update.js', (err, file) => {
        res.end(file);
    });
})









app.get('/searchmar', checkAuth, (req, res) => {
    let searchTerm = req.query.m;
    let sql = `
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
    WHERE m.title LIKE ? OR m.marketpost LIKE ? OR u.username LIKE ?;`;

    let query = `%${searchTerm}%`;

    db.query(sql, [query, query, query], (err, results) => {
        if (err) throw err;
        res.render('marketres', { results: results, searchKeyword: searchTerm });
    });
});




app.get('/marketpost/:postID',checkAuth, marketroute);
app.get('/getpic/:postID', checkAuth,marketroute);
app.post('/updatemarketpost', checkAuth,marketroute);
app.get('/delmarket/:posztID/:curuserID', checkAuth,marketroute)
app.get('/download/desktopapp',checkAuth,marketroute)





//chat
app.get('/chat', checkAuth, chatroute)
app.get('/chat.css', chatroute)
app.get('/chat.js', chatroute)
app.get('/profilepic.png', chatroute)
app.get('/get-all-chat/:mode/:uid', chatroute)
app.get('/getthischat/:id', chatroute)
app.post('/addmessage', chatroute)








//404-es oldal
app.use((req, res, next) => {
    req.url === '/404' && req.method === 'GET'
    fs.readFile('./FrontEnd/views/404.html', (err, file) => {
        res.setHeader('content-type', 'text/html');
        res.end(file);
    });
})
app.get('/404', (req, res) => {
    fs.readFile('./FrontEnd/views/404.html', (err, file) => {
        if (err) {
            res.status(500).send('Internal Server Error');
            return;
        }
        res.setHeader('content-type', 'text/html');
        res.end(file);
    });
});













let options;
try {
    options = {
        key: fs.readFileSync('/etc/letsencrypt/live/bytebag.hu/privkey.pem'),
        cert: fs.readFileSync('/etc/letsencrypt/live/bytebag.hu/fullchain.pem')
    };
} catch (error) {
    
}



const port = 80;
app.listen(3000, () => {
    console.log(`A szerver fut a http://localhost:3000`);
});



// // Az HTTPS szerver indítása a 443-as porton
// const httpsServer = https.createServer(options, app);
// httpsServer.listen(443, () => {
//     console.log('HTTPS szerver fut a 443-as porton');
// });
// const httpServer = http.createServer((req, res) => {
//     res.writeHead(301, { Location: 'https://bytebag.hu'});
//     res.end();
// });
// httpServer.listen(80, () => {
//     console.log('HTTP szerver fut a 80-as porton, átirányítva HTTPS-re');
// });
