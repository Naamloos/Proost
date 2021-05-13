var roomcode = "";

function StartGameScript() {
    document.getElementById("roomcode").innerHTML = '<i id="copyicon" class="fas fa - copy"></i> Code: ' + roomcode;
    var gamescript = document.createElement("script");
    gamescript.src = "/js/game.js";
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

        window.messageBus.send(e.senderId, 'ok');

        switch (e.type) {
            case "start":
                roomcode = e.code;
                StartGameScript();
                break;

            default:
                window.messageBus.send(e.senderId, 'huh');
        }
    };

    // initialize CastReceiverManager
    window.castReceiverManager.start({ statusText: 'Starting Proost Caster..' });
};