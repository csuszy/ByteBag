try {
    document.getElementById('verifyform').addEventListener("submit", async function(e) {
        e.preventDefault();
    
        const sendcode = {
          kod: document.getElementById('kod').value,
          pass: document.getElementById('pass').value,
          pass2: document.getElementById('pass2').value,
        };
        sendupdtae(sendcode);
    });
    
    async function sendupdtae(newData) {
      const response = await fetch('/verify-code', {
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
            text: 'Hibás vagy lejárt kód!',
            background: '#3A0061',
            color: '#fff',
        });
    }
    if (response.ok) {
        Swal.fire({
            title: 'Siker',
            text: 'A jelszó sikeresen frissítve lett!',
            icon: 'success',
            confirmButtonText: 'Bezárás',
            background: '#3A0061',
            color: '#fff',
            allowOutsideClick: false
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = '/';
            }
        });
    }
}
    
} catch (error) {
    
}