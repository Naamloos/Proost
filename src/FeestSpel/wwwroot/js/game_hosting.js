document.getElementById('container').addEventListener("click", function () {

	ws.send("++");
}, false);

document.getElementById('roomcode').addEventListener("click", function (ev) {
	ev.stopPropagation(); //stops element from being clickable, thanks to https://stackoverflow.com/a/33657471
}, false);

document.getElementById('stoptag').addEventListener("click", function (ev) {
	ev.stopPropagation();
	ws.send("xx");
}, false);