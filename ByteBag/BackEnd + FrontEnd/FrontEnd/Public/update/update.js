let logineduserID = "";
let loginedusername = "";
let loginedemail = "";
let logineddate = "";
fetch('/get-session-data')
    .then(response => response.json())
    .then(data => {
        if (data.user) {
            logineduserID = data.user.userID,
            loginedusername = data.user.username
            loginedemail = data.user.email
            logineddate = data.user.registerDATE
        } else {
            logout()
        }
});
function setToSafe(unsafe) {
  return unsafe
      .replace(/&/g, "&amp;")
      .replace(/</g, "&lt;")
      .replace(/>/g, "&gt;")
      .replace(/"/g, "&quot;")
      .replace(/'/g, "&#039;");
}

function logout() {
    fetch('/logout', {
      method: 'GET',
    }).then(response => {
      if (response.ok) {
        window.location.href = "/";
      }
    });
}





window.addEventListener("load", (event) => {
  getpost();
});
async function getpost(){
  const response = await fetch('/update/all');
  const poszt = await response.json();
  let postHTML = "";
  try {
    postHTML += `
        <a href="#" class="cardlink" style="margin-bottom: 40px;">
          <div class="card">
            <div class="card-body">
              <div class="">Azonosító</div>
              <div class="">Verziószám</div>
              <div class="">Filenév</div>
              <div class="">Frissítési jegyzet</div>
            </div>
          </div>
        </a>
      `;
    for (let post of poszt) {
      postHTML += `
        <a href="/update/${post.downloadUrl}" class="cardlink">
          <div class="card">
            <div class="card-body">
              <div class="">#${post.id}</div>
              <div class="">V${post.version}</div>
              <div class="">${post.downloadUrl}</div>
              <div class="">${post.releaseNotes}</div>
            </div>
          </div>
        </a>
      `;
    }
  } catch (error) {
    document.getElementById('allpost').innerHTML += "Még nincsenek verziók";
  }
  document.getElementById('allpost').innerHTML += postHTML;
  document.getElementById('load').style.display = "none";
}

// A modal és a gombok elemek lekérése
var modal = document.getElementById("myModal");
var cancelButton = document.getElementById("cancelButton");
var okButton = document.getElementById("okButton");
var openModalButton = document.getElementById("openModalButton");

// A modal megnyitása
openModalButton.onclick = function() {
  modal.style.display = "block";
}

// A modal bezárása a "Mégse" gombbal

function closeModal() {
  modal.style.display = "none";
}




document.getElementById('modalForm').addEventListener('submit', function (e) {
  e.preventDefault();
  const formData = new FormData(this);
  //console.log(formData);
  document.getElementById('cancelButton').disabled = true;
  document.getElementById('okButton').disabled = true;

  fetch('/uploadNewVersion', {
      method: 'POST',
      body: formData,
  })
  .then(response => {
      if (response.ok) {
        Swal.fire({
            title: 'Siker',
            text: 'A frissítés sikeresen fel lett töltve!',
            icon: 'success',
            confirmButtonText: 'Bezárás',
            background: '#3A0061',
            color: '#fff',
            allowOutsideClick: false
        }).then((result) => {
            if (result.isConfirmed) {
              document.getElementById('cancelButton').disabled = false;
              document.getElementById('okButton').disabled = false;
              window.location.href = '/update';
            }
        });
      } else {
        Swal.fire({
          icon: 'error',
          title: 'Hiba!',
          text: 'Ismeretle hiba történt! Kérjük próbálja meg később!',
          background: '#3A0061',
          color: '#fff',
        });
          document.getElementById('cancelButton').disabled = false;
          document.getElementById('okButton').disabled = false;
      }
  })
  .catch(error => {
    Swal.fire({
      icon: 'error',
      title: 'Hiba!',
      text: 'Ismeretle hiba történt! Kérjük próbálja meg később!',
      background: '#3A0061',
      color: '#fff',
    });
  });
  
  document.getElementById('cancelButton').disabled = false;
  document.getElementById('okButton').disabled = false;
});


async function seeprofil(){
  document.getElementById('profilModal').style.display = "block";
  //document.getElementById('profilid').innerHTML = `Profil: \t ID[${logineduserID}]`;
  document.getElementById('nowusername').value = loginedusername;
  document.getElementById('email').value = loginedemail;
}
   
function closeProfil() {
  document.getElementById('profilModal').style.display = "none";
}

document.getElementById('profilForm').addEventListener("submit", async function(e) {
  e.preventDefault();

  const updateThePassword = {
    userID: logineduserID,
    updatepassword: setToSafe(document.getElementById('updatepassword').value),
  };
  sendupdtae(updateThePassword);
});

async function sendupdtae(newData) {
  const response = await fetch('/updatepass', {
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
      text: 'Ismeretle hiba történt! Kérjük próbálja meg később!',
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
            document.getElementById('profilModal').style.display = "none";
        }
    });
  }
}

function gohome(){
  window.location.href = '/home';
}

function chat() {
  window.location.href = '/chat';
}

document.getElementById('gotomarket').addEventListener("click", function(){
  window.location.href = '/market'
})