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
  const response = await fetch('/get-market-ad');
  const poszt = await response.json();
  let postHTML = "";
  const d = new Date();
  try {
    for (let post of poszt) {
      const date = new Date(post.marketDATE);
      let postcall = post.title;
      let truncatedPost = postcall.length > 50 ? postcall.substring(0, 50) + ' ...' : postcall;
      //console.log(postcall);
  
      if (post.path == null) {
        postHTML += `
          <a href="/marketpost/${post.marketID}" class="cardlink">
            <div class="card">
              <div class="card-head">${post.title}</div>
              <div class="card-body">${truncatedPost}</div>
              <div class="card-foot">
                <div class="author">${post.username}</div>
                <div class="author">${post.price}FT</div>
                <div class="time">${date.toLocaleDateString("hu-HU") + " " + date.toLocaleTimeString("hu-HU", {hour:'numeric',minute:'numeric'})}</div>
              </div>
            </div>
          </a>
        `;
      }
      else{
        if (post.price <= 0) {

          postHTML += `
            <a href="/marketpost/${post.marketID}" class="cardlink">
              <div class="card">
                <div class="card-head"><img src="images/${post.path}" alt=""></img></div>
                <div class="card-body">${truncatedPost}</div>
                <div class="card-foot">
                  <div class="author">Ingyenes</div>
                  <div class="time">${post.username}</div>
                  <div class="time">${date.toLocaleDateString("hu-HU") + " " + date.toLocaleTimeString("hu-HU", {hour:'numeric',minute:'numeric'})}</div>
                </div>
              </div>
            </a>
          `;
          
        } else {
          postHTML += `
            <a href="/marketpost/${post.marketID}" class="cardlink">
              <div class="card">
                <div class="card-head"><img src="images/${post.path}" alt=""></img></div>
                <div class="card-body">${truncatedPost}</div>
                <div class="card-foot">
                  <div class="author">${post.price}FT</div>
                  <div class="time">${post.username}</div>
                  <div class="time">${date.toLocaleDateString("hu-HU") + " " + date.toLocaleTimeString("hu-HU", {hour:'numeric',minute:'numeric'})}</div>
                </div>
              </div>
            </a>
          `;
        }
      }
        
        
    }
  } catch (error) {
    document.getElementById('allpost').innerHTML += "Még nincsenek hirdetések";
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

  const imageFiles = formData.getAll('images'); // 'imageField' cseréld ki az input nevére
  for (const file of imageFiles) {
    if (!file.type.startsWith('image/')) {
      Swal.fire({
        icon: 'error',
        title: 'Hiba!',
        text: 'Csak képeket tölthetsz fel!!',
        background: '#3A0061',
        color: '#fff',
      });
      document.getElementById('cancelButton').disabled = false;
      document.getElementById('okButton').disabled = false;
      return;
    }
  }
  if (imageFiles.length > 10) {
    Swal.fire({
      icon: 'error',
      title: 'Hiba!',
      text: 'Legfeljebb 10 képet tölthetsz csak fel!',
      background: '#3A0061',
      color: '#fff',
    });
    document.getElementById('cancelButton').disabled = false;
    document.getElementById('okButton').disabled = false;
    return;
  }

  fetch('/upload', {
      method: 'POST',
      body: formData,
  })
  .then(response => {
      if (response.ok) {
        Swal.fire({
            title: 'Siker',
            text: 'A hirdetés sikeresen közzé lett téve!',
            icon: 'success',
            confirmButtonText: 'Bezárás',
            background: '#3A0061',
            color: '#fff',
            allowOutsideClick: false
        }).then((result) => {
            if (result.isConfirmed) {
              document.getElementById('cancelButton').disabled = false;
              document.getElementById('okButton').disabled = false;
              window.location.href = '/market';
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