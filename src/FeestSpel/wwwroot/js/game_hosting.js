document.getElementById('container').addEventListener("click", function () { NextPage(); }, false);

document.getElementById('roomcode').addEventListener("click", function (ev) {
    ev.stopPropagation(); //stops element from being clickable, thanks to https://stackoverflow.com/a/33657471
}, false);

document.getElementById('castbutton').addEventListener("click", function (ev) {
    ev.stopPropagation();
}, false);

document.getElementById('stoptag').addEventListener("click", function () { StopGame(); }, false);

var keydown = false;

document.addEventListener('keyup', event => {
    if (event.code === 'Space' || event.code === 'Enter') {
        keydown = false;
    }
});

document.addEventListener('keydown', event => {
    if (event.code === 'Space' || event.code === 'Enter') {
        if (!keydown) {
            keydown = true;
            NextPage();
        }
    }
});

function NextPage() {
    ws.send("++");
}

function StopGame() {
    ws.send("xx");
}