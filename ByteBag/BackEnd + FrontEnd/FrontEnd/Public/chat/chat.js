let logineduserID = "";
let loginedusername = "";
let loginedemail = "";
let logineddate = "";
let currentChatID = 0;


fetch('/get-session-data')
    .then(response => response.json())
    .then(data => {
        if (data.user) {
            logineduserID = data.user.userID,
            loginedusername = data.user.username
            loginedemail = data.user.email
            logineddate = data.user.registerDATE
            
            getchaticon();
        } else {
            logout();
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


setInterval(async function () {
  const response = await fetch(`/getthischat/${currentChatID}`);
  const thischat = await response.json();

  document.getElementById("messages").innerHTML = "";

    const messageInput = document.getElementById("messageInput");
    const messagesContainer = document.getElementById("messages");
  thischat.forEach(thismessage => {
    const messageText = thismessage.message;
    const messageElement = document.createElement("div");
    const messageposition = document.createElement("div");
    messageposition.classList.add("messageposition");
    messageElement.classList.add("uzenetdiv");
    messageElement.innerText = messageText;
    messageposition.appendChild(messageElement);

    if (thismessage.userID == logineduserID) {
      messageposition.classList.add("isendthis");
    }
    
    messagesContainer.appendChild(messageposition);
    
      
  });
  
  var scrolDownIcon = document.getElementById("scrollDown");
    if (messagesContainer.scrollHeight - Math.round(messagesContainer.scrollTop) < 950) {
      messagesContainer.scrollTop = messagesContainer.scrollHeight;
      scrolDownIcon.style.display = "none";
    }
    else{
      scrolDownIcon.style.display = "flex";
    }
}, 2000);

function scrollDown() {
  const messagesContainer = document.getElementById("messages");
  messagesContainer.scrollTop = messagesContainer.scrollHeight;
  var scrolDownIcon = document.getElementById("scrollDown");
  scrolDownIcon.style.display = "none";
}


document.getElementById('chatoption').addEventListener("change", function (e) {
  getchaticon();
})

async function getchaticon(){
  let zenmod = document.getElementById('chatoption').checked;
  if (zenmod == false) {
    const response = await fetch(`/get-all-chat/vasarlas/${logineduserID}`);
    const poszt = await response.json();
    let postHTML = "";
    const d = new Date();
    try {
      for (let post of poszt) {
          postHTML += `
              <div class="chatbutton">
                  <button class="" onclick="loadchat(${post.chatID})" id="${post.chatID}">
                      <img src="/profilepic.png" alt="">
                      <h5 id="${post.chatID}name">${post.marketUsername}</h5>
                      <p id="${post.chatID}title">${post.title}</p>
                  </button>
              </div>
          `;
        }
    } catch (error) {
        
    }
    document.getElementById('chatikonok').innerHTML = "";
    document.getElementById('chatikonok').innerHTML += postHTML;
  } else {
    const response = await fetch(`/get-all-chat/eladas/${logineduserID}`);
    const poszt = await response.json();
    let postHTML = "";
    const d = new Date();
    try {
      for (let post of poszt) {
          postHTML += `
              <div class="chatbutton">
                  <button class="" onclick="loadchat(${post.chatID})" id="${post.chatID}">
                      <img src="/profilepic.png" alt="">
                      <h5>${post.userUsername}</h5>
                      <p>${post.title}</p>
                  </button>
              </div>
          `;
        }
    } catch (error) {
        
    }
    document.getElementById('chatikonok').innerHTML = "";
    document.getElementById('chatikonok').innerHTML += postHTML;
  }
}


//userID chatID message sendDATE
async function loadchat(id) {
    const response = await fetch(`/getthischat/${id}`);
    const thischat = await response.json();
    try {
      document.getElementById(currentChatID).classList.remove("activebutton");
    } catch (error) {
      
    }
    currentChatID = id;
    document.getElementById(currentChatID).classList.add("activebutton");
    document.getElementById("messages").innerHTML = "";

    document.getElementById("chatTitle").textContent = document.getElementById(`${id}title`).textContent;
    document.getElementById("userName").textContent = document.getElementById(`${id}name`).textContent;

    thischat.forEach(thismessage => {
      const messageInput = document.getElementById("messageInput");
      const messagesContainer = document.getElementById("messages");
      const messageText = thismessage.message;
      const messageElement = document.createElement("div");
      const messageposition = document.createElement("div");
      messageposition.classList.add("messageposition");
      messageElement.classList.add("uzenetdiv");
      messageElement.innerText = messageText;
      messageposition.appendChild(messageElement);

      if (thismessage.userID == logineduserID) {
        messageposition.classList.add("isendthis");
      }
      
      messagesContainer.appendChild(messageposition);
      messageInput.value = "";
      messagesContainer.scrollTop = messagesContainer.scrollHeight;
    });
}




var cal = 0;
var uzenet = "";

function sendMessage() {
    const messageForm = document.getElementById("messageForm");
    const messageInput = document.getElementById("messageInput");
    const messagesContainer = document.getElementById("messages");
    const messageText = messageInput.value.trim();

  if (messageText !== "") {
    const messageElement = document.createElement("div");
    const messageposition = document.createElement("div");
    messageposition.classList.add("messageposition");
    messageElement.classList.add("uzenetdiv");
    messageElement.innerText = messageText;
    messageposition.appendChild(messageElement);
    messageposition.classList.add("isendthis");

    messagesContainer.appendChild(messageposition);
    messageInput.value = "";
    messagesContainer.scrollTop = messagesContainer.scrollHeight;
    uzenet = messageText;
    sendthismessage();
    uzenet = "";
  }
  else{
    Swal.fire({
      icon: 'error',
      title: 'Hiba!',
      text: 'Üres mező!',
      background: '#3A0061',
      color: '#fff',
      allowOutsideClick: false,
    });
  }
}



function sendthismessage(event) {
  const postData = {
    userID: logineduserID,
    chatID: currentChatID,
    message: setToSafe(uzenet),
  };

  document.getElementById('messageInput').value = "";
  sendComment(postData);
}

async function sendComment(newData) {
  const response = await fetch(`/addmessage`, {
      method: 'POST',
      headers: {
          'Content-Type': 'application/json',
      },
      body: JSON.stringify(newData),
  });

  if (response.status == 200) {
    loadchat(currentChatID)
  }
  else {
    Swal.fire({
      icon: 'error',
      title: 'Hiba!',
      text: 'Ismeretlen hiba történt! Kérjük próbálja meg később!',
      background: '#3A0061',
      color: '#fff',
      allowOutsideClick: false,
    });
  }
}











document.getElementById("messageForm").addEventListener("submit", function (e) {
  e.preventDefault();
  sendMessage();
});

document.getElementById("messageInput").addEventListener("keydown", function (e) {
  if (e.key === "Enter" && !e.shiftKey) {
    e.preventDefault();
    sendMessage();
  }
});