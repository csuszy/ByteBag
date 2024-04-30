let logineduserID = "";
let loginedusername = "";
let loginedemail = "";
let logineddate = "";
let isAdmin = false;
fetch('/get-session-data')
  .then(response => response.json())
  .then(data => {
    if (data.user) {
      logineduserID = data.user.userID,
      loginedusername = data.user.username
      loginedemail = data.user.email
      logineddate = data.user.registerDATE
      if (data.user.admin == 1) {
        isAdmin = true;
      }
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
async function getpost() {
  const response = await fetch('/getpost');
  const poszt = await response.json();
  let postHTML = "";
  const d = new Date();
  try {
    for (let post of poszt) {
      const date = new Date(post.postDATE);
      let postcall = post.post;
      let truncatedPost = postcall.length > 50 ? postcall.substring(0, 50) + ' ...' : postcall;

      postHTML += `
        <a href="/post/${post.posztID}" class="cardlink">
          <div class="card">
            <div class="card-head">${post.title}</div>
            <div class="card-body">${truncatedPost}</div>
            <div class="card-foot">
              <div class="author">${post.username}</div>
              <div class="time">${date.toLocaleDateString("hu-HU") + " " + date.toLocaleTimeString("hu-HU", { hour: 'numeric', minute: 'numeric' })}</div>
            </div>
          </div>
        </a>
        `;
    }

  } catch (error) {

  }
  document.getElementById('allpost').innerHTML += postHTML;
  document.getElementById('load').style.display = "none";
}
let createmodal = `
        <div class="modal-content">
          <form id="modalForm">
            <div class="form-group">
              <label for="username">Cím:</label>
              <input type="text" id="username" name="username" maxlength="150" required>
            </div>
            <div class="form-group">
              <label for="post">Tartalom:</label><br>
              <textarea type="post" id="post" name="post" required></textarea>
            </div>
            <div class="button-group">
              <button type="button" onclick="closeModal()" id="cancelButton">Mégse</button>
              <button type="submit" id="okButton">Posztolás</button>
            </div>
          </form>
        </div>
`;
document.getElementById('myModal').innerHTML = createmodal;


// A modal és a gombok elemek lekérése
var modal = document.getElementById("myModal");
var cancelButton = document.getElementById("cancelButton");
var okButton = document.getElementById("okButton");
var openModalButton = document.getElementById("openModalButton");
var form = document.getElementById("modalForm");

// A modal megnyitása
openModalButton.onclick = function () {
  modal.style.display = "block";
}

// A modal bezárása a "Mégse" gombbal

function closeModal() {
  modal.style.display = "none";
}

// A modal form "OK" gombbal
form.addEventListener("submit", function (e) {
  e.preventDefault();
  let newtitle = document.getElementById("username").value;
  let newpost = document.getElementById("post").value;

  if (newtitle.length > 150) {
    Swal.fire({
      icon: 'error',
      title: 'Hiba!',
      text: 'A cím maximális hossza nem lehet több mint 150 karakter!',
      background: '#3A0061',
      color: '#fff',
    });
  } else {
    okButton.disabled = true;
    const postData = {
      userID: logineduserID,
      newtitle: setToSafe(newtitle),
      newpost: setToSafe(newpost),
    };
    sendpost(postData);
    modal.style.display = "none";
  }
});



async function sendpost(newData) {
  const response = await fetch('/newpost', {
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
    okButton.disabled = false;
  }
  if (response.ok) {
    Swal.fire({
      title: 'Siker',
      text: 'A poszt sikeresen közzé lett téve!',
      icon: 'success',
      confirmButtonText: 'Bezárás',
      background: '#3A0061',
      color: '#fff',
      allowOutsideClick: false
    }).then((result) => {
      if (result.isConfirmed) {
        window.location.href = '/home';
      }
    });
  }
}


async function seeprofil() {
  document.getElementById('profilModal').style.display = "block";
  //document.getElementById('profilid').innerHTML = `Profil: \t ID[${logineduserID}]`;
  document.getElementById('nowusername').value = loginedusername;
  document.getElementById('email').value = loginedemail;
  if (isAdmin === true) {
    document.getElementById("downloadlink").innerHTML = `
      <a href="/update">Verziok<i class="fa-solid fa-cloud-arrow-down"></i></a>
    `;
  }
}

function closeProfil() {
  document.getElementById('profilModal').style.display = "none";
}

document.getElementById('profilForm').addEventListener("submit", async function (e) {
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

function gohome() {
  window.location.href = '/home';
}
function chat() {
  window.location.href = '/chat';
}
document.getElementById('gotomarket').addEventListener("click", function () {
  window.location.href = '/market'
})