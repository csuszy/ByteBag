const fs = require("fs");
const express = require("express");
const route = express.Router();
const pool = require("./db");

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

route.get("/post.css", (req, res) => {
  fs.readFile("./FrontEnd/Public/post/post.css", (err, file) => {
    res.end(file);
  });
});
route.get("/post.js", (req, res) => {
  fs.readFile("./FrontEnd/Public/post/post.js", (err, file) => {
    res.end(file);
  });
});
route.get("/post/:postID", checkAuth, (req, res, next) => {
  const postId = req.params.postID;
  //console.log(postId);

  pool.query(
    "SELECT forum.posztID, forum.userID, user.userID, user.username, forum.title, user.userID, TRIM(forum.post) AS post, forum.hashtag, forum.postDATE FROM forum LEFT JOIN user ON forum.userID = user.userID WHERE forum.posztID = ?;",
    [postId],
    function (err, results) {
      if (err) {
        //console.log("asd");
        return callback(err);
      } else {
        //console.log(results[0]);
        try {
          return res.render("post", { post: results[0] });
        } catch (error) {
          res.redirect("/404");
        }
      }
    }
  );
});

route.get("/delpost/:posztID/:curuserID", checkAuth, (req, res, next) => {
  const postID = req.params.posztID;
  const senduserID = req.params.curuserID;

  if (req.session.user.userID == senduserID || req.session.user.admin == 1) {
    
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
  } else {
    res.status(401).send();
  }
});

route.post("/updatepost", checkAuth, (req, res, next) => {
  const { userPosztID, editposztID, newtitle, newpost } = req.body;

  if (req.session.user.userID == userPosztID || req.session.user.admin == 1) {
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
  } else {
    res.status(401).send();
  }
});
route.post("/sendcomment", (req, res, next) => {
  const { commentToPosztID, commentUserID, commentText } = req.body;
  //console.table(req.body);
  const query = `INSERT INTO comment (commentID, posztID, userID, commentText, commentDATE) VALUES (NULL, ?, ?, ?, current_timestamp());`;
  pool.query(
    query,
    [commentToPosztID, commentUserID, commentText],
    (err, results) => {
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
      res.status(200).send();
    }
  );
});
route.get("/getcomment/:currentposztID", (req, res) => {
  const currentposztID = req.params.currentposztID;
  const query = `
    SELECT comment.commentID, comment.posztID, comment.userID, user.username, comment.commentText, comment.commentDATE
    FROM comment
	LEFT JOIN user ON comment.userID = user.userID
    WHERE comment.posztID = ?
    ORDER BY comment.commentDATE DESC;

    `;
  pool.query(query, [currentposztID], (err, results) => {
    if (err) {
      //console.error(err);
      res.status(500).send({ message: "Szerver hiba!" });
      return;
    }
    res.setHeader("content-type", "application/json");
    res.json(results);
  });
});

module.exports = route;
