const { createTransport } = require('nodemailer');

const transporter = createTransport({
    host: "",
    port: 465,
    secure: true,
    auth: {
        user: "",
        pass: "",
    },
});

module.exports = transporter;