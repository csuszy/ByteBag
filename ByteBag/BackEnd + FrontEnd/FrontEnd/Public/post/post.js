var logineduserID = "";
let loginedusername = "";
let loginedemail = "";
let logineddate = "";
let currentPosztID = "";
let userPosztID = "";
let isAdmin = false;
fetch("/get-session-data")
  .then((response) => response.json())
  .then((data) => {
    if (data.user) {
      setCurrentUser(data.user);
    } else {
      logout();
    }
  });
async function setCurrentUser(sessiondata) {
  (logineduserID = sessiondata.userID),
    (loginedusername = sessiondata.username);
  loginedemail = sessiondata.email;
  logineddate = sessiondata.registerDATE;
  if (sessiondata.admin == 1) {
    isAdmin = true;
    console.log("Admin? " + isAdmin);
  }

  await editBTN();
  currentPosztID = document.querySelector("#modifyBTN").dataset.posztid;
  userPosztID = document.querySelector("#modifyBTN").dataset.posztuserid;
  console.log(currentPosztID);

  const response = await fetch(`/getcomment/${currentPosztID}`);
  const poszt = await response.json();
  if (poszt.length == 0) {
    document.getElementById("commentsection").innerHTML +=
      "Jelenleg még nincs egy komment sem. Légy te az első!";
  }
  let postHTML = "";
  for (let comment of poszt) {
    var regex = /(https?:\/\/\S+)/;
    if (comment.commentText.match(regex)) {
      
      // A link kinyerése a szövegből
      var eredmeny = comment.commentText.match(regex);
      postHTML += `
        <div class="usercomment">
          <div class="user">${comment.username}</div>
          <div class="comment">${comment.commentText} <br> <a href="${eredmeny[0]}" target="_blank" class="commentLink" rel="noopener noreferrer">${eredmeny[0]}</a></div>
        </div>
      `;
    }
    else{
      postHTML += `
        <div class="usercomment">
          <div class="user">${comment.username}</div>
          <div class="comment">${comment.commentText}</div>
        </div>
      `;
    }
  }
  document.getElementById("commentsection").innerHTML += postHTML;
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
  fetch("/logout", {
    method: "GET",
  }).then((response) => {
    if (response.ok) {
      window.location.href = "/";
    }
  });
}

async function seeprofil() {
  document.getElementById("profilModal").style.display = "block";
  document.getElementById(
    "profilid"
  ).innerHTML = `Profil: \t ID[${logineduserID}]`;
  document.getElementById("nowusername").value = loginedusername;
  document.getElementById("email").value = loginedemail;
}

function closeProfil() {
  document.getElementById("profilModal").style.display = "none";
}

document
  .getElementById("profilForm")
  .addEventListener("submit", async function (e) {
    e.preventDefault();

    const updateThePassword = {
      userID: logineduserID,
      updatepassword: setToSafe(
        document.getElementById("updatepassword").value
      ),
    };
    sendupdtae(updateThePassword);
  });

async function sendupdtae(newData) {
  const response = await fetch("/updatepass", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(newData),
  });

  if (response.status == 401) {
    Swal.fire({
      icon: "error",
      title: "Hiba!",
      text: "Ismeretle hiba történt! Kérjük próbálja meg később!",
      background: "#3A0061",
      color: "#fff",
    });
  }
  if (response.ok) {
    Swal.fire({
      title: "Siker",
      text: "A jelszó sikeresen frissítve lett!",
      icon: "success",
      confirmButtonText: "Bezárás",
      background: "#3A0061",
      color: "#fff",
      allowOutsideClick: false,
    }).then((result) => {
      if (result.isConfirmed) {
        window.location.href = "/home";
        document.getElementById("profilModal").style.display = "none";
      }
    });
  }
}

function gohome() {
  window.location.href = "/home";
}

function chat() {
  window.location.href = "/chat";
}

function editBTN() {
  let getid = document.querySelector("#poszt");
  if (getid.dataset.id == logineduserID || isAdmin == true) {
    let button = `
    <button onclick="delPost()"><i class="fa-regular fa-trash-can"></i></button>
    <button onclick="szerkesztPost()"><i class="fa-regular fa-pen-to-square"></i></button>
    `;
    document.getElementById("modifyBTN").innerHTML = button;
  }
}

// A modal megnyitása
function szerkesztPost() {
  document.getElementById("edittitle").value = document
    .getElementById("title")
    .textContent.trim();
  document.getElementById("editpost").value = document
    .getElementById("post")
    .textContent.trim();
  document.getElementById("myModal").style.display = "block";
}

// A modal bezárása a "Mégse" gombbal

function closeModal() {
  document.getElementById("myModal").style.display = "none";
}

// A modal form "OK" gombbal
document.getElementById("modalForm").addEventListener("submit", function (e) {
  e.preventDefault();
  let newtitle = document.getElementById("edittitle").value;
  let newpost = document.getElementById("editpost").value;
  const postData = {
    userPosztID: userPosztID,
    editposztID: currentPosztID,
    newtitle: setToSafe(newtitle).trim(),
    newpost: setToSafe(newpost).trim(),
  };
  updatePOST(postData);
  modal.style.display = "none";
});
async function updatePOST(newData) {
  const response = await fetch(`/updatepost`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(newData),
  });

  if (response.status == 401) {
    Swal.fire({
      icon: "error",
      title: "Hiba!",
      text: "Ismeretle hiba történt! Kérjük próbálja meg később!",
      background: "#3A0061",
      color: "#fff",
    });
  }
  if (response.ok) {
    Swal.fire({
      title: "Siker",
      text: "A poszt sikeresen frissítve lett!",
      icon: "success",
      confirmButtonText: "Bezárás",
      background: "#3A0061",
      color: "#fff",
      allowOutsideClick: false,
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
    background: "#3A0061",
    color: "#fff",
  }).then(async (result) => {
    if (result.isConfirmed) {
      const response = await fetch(
        `/delpost/${currentPosztID}/${userPosztID}`,
        {}
      );
      if (response.status == 200) {
        Swal.fire({
          title: "Siker!",
          text: "A poszt sikeresen törölve lett!",
          confirmButtonText: "Bezárás",
          icon: "success",
          background: "#3A0061",
          allowOutsideClick: false,
          color: "#fff",
        }).then((result) => {
          if (result.isConfirmed) {
            window.location.href = "/home";
          }
        });
      }
      if (response.status == 401) {
        Swal.fire({
          icon: "error",
          title: "Hiba!",
          text: "Ismeretle hiba történt! Kérjük próbálja meg később!",
          background: "#3A0061",
          color: "#fff",
          allowOutsideClick: false,
        });
      }
    }
  });
}

function postcomment(event) {
  event.preventDefault();

  if (setToSafe(document.getElementById("newcomment").value).length < 6) {
    Swal.fire({
      title: "Minimum 6 karakter szükséges!",
      text: "A komment közzétételéhez minimum 6 karakter szükséges.",
      icon: "error",
      background: "#3A0061",
      color: "#fff",
      confirmButtonText: "Bezárás",
    });
  } else {
    const postData = {
      commentToPosztID: currentPosztID,
      commentUserID: logineduserID,
      commentText: setToSafe(document.getElementById("newcomment").value),
    };
    document.getElementById("newcomment").value = "";
    sendComment(postData);
  }
}

async function sendComment(newData) {
  const response = await fetch(`/sendcomment`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(newData),
  });

  if (response.status == 401) {
    Swal.fire({
      icon: "error",
      title: "Hiba!",
      text: "Ismeretle hiba történt! Kérjük próbálja meg később!",
      background: "#3A0061",
      color: "#fff",
      allowOutsideClick: false,
    });
  }
  if (response.status == 200) {
    location.reload();
  }
}

document.getElementById("gotomarket").addEventListener("click", function () {
  window.location.href = "/market";
});

async function getPic() {
  currentPosztID = document.querySelector("#modifyBTN").dataset.posztid;
  console.log(currentPosztID);

  const response = await fetch(`/getpic/${currentPosztID}`);
  const pic = await response.json();
  if (pic.length == 0) {
    document.getElementById("kepek").innerHTML += "-";
  }
  let postHTML = "";

  pic.forEach((element) => {
    postHTML += `
      <img src="/images/${element.path}" alt="${element.path}">
      `;
  });

  document.getElementById("kepek").innerHTML += postHTML;
}
window.addEventListener("load", (event) => {
  getPic();
});
