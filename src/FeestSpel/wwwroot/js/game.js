if (navigator.share) {
    document.getElementById("copyicon").className = "fas fa-share";
}

var roomurl = location.protocol + '//' + location.host + location.pathname + "?" + roomcode;

var qrcode = new QRCode(document.getElementById("qr"), {
    text: roomurl,
    width: 125,
    height: 125,
    colorDark: "#000000",
    colorLight: "#FFFFFF",
    correctLevel: QRCode.CorrectLevel.H
});

//Copy the roomcode to the clipboard
function copyRoomcodeToClipBoard() {
    let roomcode = document.getElementById('hiddenRoomCodeField');
    roomcode.value = roomurl;
    roomcode.type = 'text'; //document.execCommand('copy') doesn't work on hidden items
    roomcode.focus();
    roomcode.select();
    try {
        if (navigator.share) {
            // Web Share API is supported
            navigator.share({
                title: "Proost! (" + roomcode + ")",
                url: roomurl
            }).then(() => {
                console.log('Shared room url!');
            });
        } else {
            // Fallback
            var successful = document.execCommand('copy');
            var roomtag = document.getElementById("roomcode");
            var old = roomtag.innerHTML;
            var width = roomtag.clientWidth;
            roomtag.innerHTML = "Gekopieerd!";
            roomtag.clientWidth = width;
            setInterval(() => { roomtag.innerHTML = old; }, 2000);
        }
        console.log('Copied or shared roomcode succesfully');
    } catch (err) {
        console.log('Copied or shared roomcode unsuccesfully');
    }
    roomcode.type = 'hidden';
}

var colors = ["#fcba03", "#03fc66", "#035afc", "#9003fc", "#fc0373", "#fc2003", "#03fcd3"];

function setBg() {
    var color = colors[Math.floor(Math.random() * colors.length)];
    document.body.style.backgroundColor = color;
    document.getElementById("headercolor").content = color;
}

setBg();

var mission = document.getElementById("mission");

var wspath = window.location.hostname + ":" + window.location.port + "/api/ws";

if (location.protocol === 'https:') {
    // page is secure
    wspath = "wss://" + wspath;
} else {
    wspath = "ws://" + wspath;
}

var ws = new WebSocket(wspath);

ws.onmessage = function (event) {
    var response = JSON.parse(event.data);

    switch (response.Action) {
        case "redirect":
            window.location.href = response.Context;
            break;

        case "text":
            mission.innerHTML = response.Context;
            setBg();
            break;

        default:
            break;
    }

    ws.send("OK");
}