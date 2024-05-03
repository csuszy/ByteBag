const mysql = require('mysql2');
try {
    const pool = mysql.createPool({
        host: 'localhost',
        port: 3306,
        user: "root",
        password: "",
        database: "s43625_szakdoga",
        waitForConnections: true,
        connectionLimit: 10,
        queueLimit: 0
    });
    
    module.exports = pool;
} catch (error) {
    console.log("Error creating connection");
}



