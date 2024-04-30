
var logineduserID = "";
let loginedusername = "";
let loginedemail = "";
let logineddate = "";

let currentPosztID = "";
let userPosztID = "";
let isAdmin = false;
let chatid = null;

fetch('/get-session-data')
  .then(response => response.json())
  .then(data => {
    if (data.user) {
      setCurrentUser(data.user);
    } else {
      logout()
    }
  });
async function setCurrentUser(sessiondata) {
  logineduserID = sessiondata.userID,
    loginedusername = sessiondata.username
  loginedemail = sessiondata.email
  logineddate = sessiondata.registerDATE
  if (sessiondata.admin == 1) {
    isAdmin = true;
  }

  await editBTN();
  currentPosztID = document.querySelector("#modifyBTN").dataset.posztid
  userPosztID = document.querySelector("#modifyBTN").dataset.posztuserid
  chatid = document.querySelector("#sendchat").dataset.chatid;
  console.log(`${currentPosztID}, ${userPosztID}, ${chatid}`);
}





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

async function seeprofil() {
  document.getElementById('profilModal').style.display = "block";
  document.getElementById('profilid').innerHTML = `Profil: \t ID[${logineduserID}]`;
  document.getElementById('nowusername').value = loginedusername;
  document.getElementById('email').value = loginedemail;
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

function editBTN() {
  let getid = document.querySelector("#poszt");
  if (getid.dataset.id == logineduserID || isAdmin == true) {
    let button = `
    <button onclick="delPost()"><i class="fa-regular fa-trash-can"></i></button>
    <button onclick="szerkesztPost()"><i class="fa-regular fa-pen-to-square"></i></button>
    `;
    document.getElementById('modifyBTN').innerHTML = button;
  }
}



// A modal megnyitása
function szerkesztPost() {
  //document.getElementById('edittitle').value = document.getElementById('title').textContent;
  document.getElementById('editpost').innerHTML = document.getElementById('post').textContent;
  document.getElementById('myModal').style.display = "block";
}

// A modal bezárása a "Mégse" gombbal

function closeModal() {
  document.getElementById("myModal").style.display = "none";
}

// A modal form "OK" gombbal
document.getElementById('modalForm').addEventListener("submit", function (e) {
  e.preventDefault();
  let newtitle = document.getElementById("edittitle").value;
  let newpost = document.getElementById("editpost").value;
  let editprice = document.getElementById("editprice").value;
  const postData = {
    userPosztID: userPosztID,
    editposztID: currentPosztID,
    newtitle: setToSafe(newtitle),
    //newpost: setToSafe(newpost),
    newpost: newpost,
    editprice: setToSafe(editprice),
  };
  updatemarketPOST(postData);
  modal.style.display = "none";
});


async function updatemarketPOST(newData) {
  const response = await fetch(`/updatemarketpost`, {
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
      text: 'A hirdetés sikeresen frissítve lett!',
      icon: 'success',
      confirmButtonText: 'Bezárás',
      background: '#3A0061',
      color: '#fff',
      allowOutsideClick: false
    }).then((result) => {
      if (result.isConfirmed) {
        location.reload();
      }
    });
  }
}



async function delPost() {
  Swal.fire({
    title: "Biztos hogy törölni szeretnéd?",
    text: "Ezt a folyamatot nem lehet vissza vonni!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonText: "Igen, törlöm!",
    cancelButtonText: "Nem, nem törlöm!",
    reverseButtons: true,
    allowOutsideClick: false,
    background: '#3A0061',
    color: '#fff',
  }).then(async (result) => {
    if (result.isConfirmed) {
      const response = await fetch(`/delmarket/${currentPosztID}/${userPosztID}`, {});
      if (response.status == 200) {
        Swal.fire({
          title: "Siker!",
          text: "A poszt sikeresen törölve lett!",
          confirmButtonText: 'Bezárás',
          icon: "success",
          background: '#3A0061',
          allowOutsideClick: false,
          color: '#fff',
        }).then((result) => {
          if (result.isConfirmed) {
            window.location.href = '/market';
          }
        });
      }
      if (response.status == 401) {
        Swal.fire({
          icon: 'error',
          title: 'Hiba!',
          text: 'Ismeretle hiba történt! Kérjük próbálja meg később!',
          background: '#3A0061',
          color: '#fff',
        });
      }
    }
  });
}


async function getPic() {
  const currentPosztID = document.querySelector("#modifyBTN").dataset.posztid;
  console.log(currentPosztID);

  const response = await fetch(`/getpic/${currentPosztID}`);
  const pic = await response.json();

  if (pic.length === 0) {
    document.getElementById('carousel-inner').innerHTML += "<div class='carousel-item active'><img src='/images/default.jpg' class='d-block' alt='Nincs kép'></div>";
  } else {
    let postHTML = pic.map((element, index) => `
        <div class="carousel-item ${index === 0 ? 'active' : ''}">
          <img src="/images/${element.path}" class="d-block" alt="...">
        </div>
      `).join('');

    document.getElementById('carousel-inner').innerHTML = postHTML;
  }
}

getPic()

document.getElementById('sendchat').addEventListener("click", async function (e) {
  e.preventDefault();
  if (chatid > 0) {
    window.location.href = '/chat';
  }
  else if(chatid == 0){
    const newData = {
      logineduserID: logineduserID,
      currentPosztID: currentPosztID,
    };
    console.log(newData);
    const response = await fetch(`/create-chat`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(newData),
    });
      if (response.status == 200) {
        window.location.href = '/chat';
      }
      if (response.status != 200) {
        Swal.fire({
          icon: 'error',
          title: 'Hiba!',
          text: 'Ismeretle hiba történt! Kérjük próbálja meg később!',
          background: '#3A0061',
          color: '#fff',
        });
      }
  }
});







document.getElementById('gotomarket').addEventListener("click", function () {
  window.location.href = '/market'
})