$('.message a').click(function () {
    $('form').animate({ height: "toggle", opacity: "toggle" }, "slow");
});


function setToSafe(unsafe) {
    return unsafe
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}

document.querySelector('#login-form').addEventListener('submit', (e) => {
    e.preventDefault();
    let logusrname = document.querySelector('#loginusername').value;
    let logpass = document.querySelector('#loginpassword').value;
    let rememberme = document.querySelector('#rememberme').checked;

    const loginData = {
        loginusername: setToSafe(logusrname),
        loginpassword: setToSafe(logpass),
        rememberme: rememberme
    };

    sendData(loginData);
});

async function sendData(newData) {
    const response = await fetch('/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(newData),
    });

    if (response.ok) {
        window.location.href = '/home';
    } else {
        Swal.fire({
            icon: 'error',
            title: 'Hiba!',
            text: 'Hibás felhasználónevet vagy jelszót adtál meg!',
            background: '#3A0061',
            color: '#fff',
        });
    }
}

document.addEventListener('DOMContentLoaded', () => {
    // Gomb eseménykezelője
    const githubLoginBtn = document.getElementById('github-login-btn');
    githubLoginBtn.addEventListener('click', () => {
        // GitHub bejelentkezési útvonal
        const githubLoginUrl = '/auth/github';
        // Átirányítás a GitHub bejelentkezési oldalra
        window.location.href = githubLoginUrl;
    });
});

document.getElementById('github-login-btn').addEventListener('click', (event) => {
    document.getElementById('load').style.display = "block";
});


window.addEventListener("load", (event) => {
    document.getElementById('load').style.display = "none";
});



function checkpass() {
    let pass = document.querySelector('#regpassword').value;
    if (pass == "Chuck Norris" || pass == "ChuckNorris" || pass == "chuck norris" || pass == "chucknorris" || pass == "CHUCK NORIS" || pass == "CHUCKNORIS") {
        Swal.fire({
            icon: 'warning',
            title: 'Chuck Norris',
            text: 'Ez a jelszó túl errős!'
        });
    }
}
function showpass() {
    let show = document.getElementById('showpass1').checked;
    if (show == true) {
        document.querySelector('#regpassword').type = 'text';
        document.querySelector('#regpasswordagain').type = 'text';
    }
    else {
        document.querySelector('#regpassword').type = 'password';
        document.querySelector('#regpasswordagain').type = 'password';
    }
}

document.querySelector('#register-form').addEventListener('submit', (e) => {
    e.preventDefault();
    let username = document.querySelector('#regusername').value;
    let email = document.querySelector('#regemail').value;
    let pass = document.querySelector('#regpassword').value;

    if (username == "" || email == "" || pass == "") {
        Swal.fire({
            icon: 'error',
            title: 'Hiba!',
            text: 'Minden mezőt ki kell tölteni!'
        });
    } else if (document.querySelector('#regpassword').value === document.querySelector('#regpasswordagain').value) {
        const registerData = {
            regusername: setToSafe(username),
            regemail: setToSafe(email),
            regpassword: setToSafe(pass)
        };
        sendregData(registerData);
    }
    else {
        Swal.fire({
            icon: 'error',
            title: 'Hiba!',
            text: 'A jelszavak nem egyeznek!'
        })
    }


});

async function sendregData(newData) {
    const response = await fetch('/register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(newData),
    });

    if (response.status == 401) {
        Swal.fire({
            icon: 'error',
            title: 'Hiba!',
            text: 'Ez az email vagy felhasználónév már foglalt!'
        });
    }
    if (response.ok) {
        Swal.fire({
            title: 'Siker!',
            text: 'Sikeres regisztráció!',
            icon: 'success',
            confirmButtonText: 'Bezárás',
            background: '#3A0061',
            color: '#fff',
            allowOutsideClick: false
        }).then((result) => {
            // A felhasználó bezárta a modalt
            if (result.isConfirmed) {
                window.location.href = '/';
            }
        });
    }
}
