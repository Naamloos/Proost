var players = 3;
var playerlist = document.getElementById("playerlist");

function addPlayer() {
    if (players >= 100) {
        alert("Speler limiet bereikt.");
        return;
    }
    players++;

    var newnode = document.createElement("input");
    newnode.value = "";
    newnode.name = "player" + players;
    newnode.placeholder = "Speler " + players + "...";
    newnode.autocomplete = "off";
    playerlist.appendChild(newnode)
}

function clearPlayers() {
    players = 0;
    playerlist.innerHTML = "";

    for (var i = 0; i < 3; i++) {
        addPlayer();
    }
}

function removeLast() {
    if (players < 4)
        return;
    players--;
    var lastchild = playerlist.children[playerlist.children.length - 1];
    playerlist.removeChild(lastchild);
}

clearPlayers();