var mission = document.getElementById("mission");
var qr = document.getElementById("qr");

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

function setCode(code) {
    var qrcode = new QRCode(qr, {
        text: location.protocol + '//' + location.host + location.pathname + "?" + code,
        width: 125,
        height: 125,
        colorDark: "#653ba2",
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
            window.messageBus.send(e.senderId, 'code ok');
            setCode(msg.code);
        }
        if (msg.type === 'newtext') {
            window.messageBus.send(e.senderId, 'received msg');
            setNewText(msg.text);
            setBg();
        }
        if (msg.type === 'kill') {
            window.messageBus.send(e.senderId, 'killing cast conn');
            setNewText("Spel voorbij");
            setBg();
            window.castReceiverManager.stop();//dont know, can't test :/ No chromecast in my student room rn.
        }
    };

    // initialize CastReceiverManager
    window.castReceiverManager.start({ statusText: 'Starting Proost Caster..' });
};