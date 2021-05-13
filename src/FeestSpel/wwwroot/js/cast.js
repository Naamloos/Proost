var mission = document.getElementById("mission");
var qr = document.getElementById("qr");
var colors = ["#fcba03", "#03fc66", "#035afc", "#9003fc", "#fc0373", "#fc2003", "#03fcd3"];

function setBg() {
    var color = colors[Math.floor(Math.random() * colors.length)];
    document.body.style.backgroundColor = color;
    document.getElementById("headercolor").content = color;
}

setBg();

function setCode(code) {
    var qrcode = new QRCode(qr, {
        text: location.protocol + '//' + location.host + location.pathname + "?" + code,
        width: 125,
        height: 125,
        colorDark: "#000000",
        colorLight: "#FFFFFF",
        correctLevel: QRCode.CorrectLevel.H
    });

    document.getElementById("roomcode").innerHTML = 'Code: ' + roomcode;
}

function setNewText(text) {
    mission.innerHTML = text;
}

// Big thanks to https://github.com/DeMille/url-cast-receiver/

window.onload = function () {
    window.castReceiverManager = cast.receiver.CastReceiverManager.getInstance();

    castReceiverManager.onReady = function (event) {
        window.castReceiverManager.setApplicationState('Ready for casting..');
    };

    // messages on a custom namespace
    var ns = 'urn:x-cast:app.proost.cast';
    window.messageBus = window.castReceiverManager.getCastMessageBus(ns);

    window.messageBus.onMessage = function (e) {
        var msg = JSON.parse(e.data);

        if (msg.type === 'start') {
            roomcode = msg.code;
            window.messageBus.send(e.senderId, 'yes thx');
            setCode(msg.code);
        }
        if (msg.type === 'newtext') {
            setNewText(msg.text);
        }
    };

    // initialize CastReceiverManager
    window.castReceiverManager.start({ statusText: 'Starting Proost Caster..' });
};