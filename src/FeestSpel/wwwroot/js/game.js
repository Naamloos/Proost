﻿if (navigator.share) {
    document.getElementById("copyicon").className = "fas fa-share";
}

var roomurl = location.protocol + '//' + location.host + location.pathname + "?" + roomcode;

var missiontext = "";

var qrcode = new QRCode(document.getElementById("qr"), {
    text: roomurl,
    width: 125,
    height: 125,
    colorDark: "#653ba2",
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
//old colors if u want them ryan
//var colors = ["#fcba03", "#03fc66", "#035afc", "#9003fc", "#fc0373", "#fc2003", "#03fcd3"];

//first color is normal color, the second color is the gradient | most colors are from https://cssgradient.io/gradient-backgrounds/
var colors = [["#FDEB71", "#F8D800"], ["#ABDCFF", "#0396FF"], ["#FEB692", "#EA5455"], ["#CE9FFC", "#7367F0"], ["#90F7EC", "#32CCBC"], ["#FFF6B7", "#F6416C"],
              ["#81FBB8", "#28C76F"], ["#E2B0FF", "#9F44D3"], ["#F97794", "#623AA2"], ["#FCCF31", "#F55555"], ["#F761A1", "#8C1BAB"], ["#43CBFF", "#9708CC"],
              ["#5EFCE8", "#736EFE"], ["#FAD7A1", "#E96D71"], ["#FFD26F", "#3677FF"], ["#A0FE65", "#FA016D"], ["#FFDB01", "#0E197D"], ["#FEC163", "#DE4313"],
              ["#FDD819", "#E80505"], ["#79F1A4", "#0E5CAD"], ["#F05F57", "#360940"], ["#2AFADF", "#4C83FF"], ["#6B73FF", "#000DFF"], ["#70F570", "#49C628"],
              ["#FFCF71", "#2376DD"]];

function setBg() {
    var randomColor = Math.floor(Math.random() * colors.length);

    var gradient = "radial-gradient(circle, " + colors[randomColor][0] + ", " + colors[randomColor][1] + ")";
    document.body.style.background = gradient;
    document.getElementById("headercolor").content = colors[randomColor][0];
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

var ws;

function reconnect() {
    ws = new WebSocket(wspath);

    ws.onmessage = function (event) {
        var response = JSON.parse(event.data);

        switch (response.Action) {
            case "redirect":
                try {
                    sendMessage({ type: 'kill', text: missiontext });
                } catch (e) { }
                try { session.endSession(true); } catch (e) { }
                window.location.href = response.Context;
                break;

            case "text":
                mission.innerHTML = response.Context;
                missiontext = response.Context;
                setBg();
                try {
                    sendMessage({ type: 'newtext', text: missiontext });
                } catch (e) { }
                break;

            default:
                break;
        }

        ws.send("OK");
    }
}

reconnect();

function sendCast() {
    // request chromecast session
    chrome.cast.requestSession(sessionListener, onErr);
}

var castbtn = document.getElementById("castbutton");

// Big thanks to https://github.com/DeMille/url-cast-receiver/
window.__onGCastApiAvailable = function (loaded) {
    if (loaded) { initializeCastApi(); castbtn.style.display = 'auto' };
};

var applicationID = 'FE21227F'
    , namespace = 'urn:x-cast:app.proost.cast'
    , session = null;

function initializeCastApi() {
    var sessionRequest = new chrome.cast.SessionRequest(applicationID);

    var apiConfig = new chrome.cast.ApiConfig(
        sessionRequest,
        sessionListener,
        receiverListener
    );

    chrome.cast.initialize(
        apiConfig,
        onSuccess.bind(this, 'initialized ok'),
        onErr
    );

    castbtn.style.display = "block";
}

function onErr(err) {
    console.log('Err: ' + JSON.stringify(err));
    showError(err);
}

function onSuccess(msg) {
    console.log('Sucess: ' + msg);
}

function sessionListener(newSession) {
    console.log('New session ID:' + newSession.sessionId);
    session = newSession;
    session.addUpdateListener(sessionUpdateListener);
    session.addMessageListener(namespace, receiveMessage);
    sendMessage({ type: "start", code: roomcode });
}

function receiverListener(e) {
    (e === 'available')
        ? console.log('receiver found')
        : console.log('no receivers found');
}

function sessionUpdateListener(isAlive) {
    if (!isAlive) {
        session = null;
    }
    console.log('Session is alive?: ', isAlive);
}

function receiveMessage(namespace, msg) {
    console.log('Chromecast: ' + msg);
    if (msg === 'code ok') {
        try {
            sendMessage({ type: 'newtext', text: missiontext });
        } catch (e) { }
    }
}

function sendMessage(msg) {
    session.sendMessage(
        namespace,
        msg,
        function () {
            console.log('Message sent: ', msg);
            notify('Message sent: ' + JSON.stringify(msg));
        },
        onErr
    );
}

function notify(msg) {
}

function showError(err) {
}

function doWebsocketCheck() {
    setTimeout(function () {
        // CLOSING or CLOSED
        if (ws.readyState == 2 || ws.readyState == 3) {
            reconnect();
        }
        doWebsocketCheck();
    }, 500);
}

doWebsocketCheck();