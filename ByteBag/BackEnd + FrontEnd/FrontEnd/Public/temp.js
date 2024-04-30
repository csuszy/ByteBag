

async function sendData() {
    const response = await fetch('/cookie-login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
    });

    console.log(response);

    if (response.status == 200) {
        window.location.href = '/home';
    }
    else if (response.status == 300) {
        window.location.href = '/logout';
    }
    else{
        window.location.href = '/logout';
    }
}

sendData();