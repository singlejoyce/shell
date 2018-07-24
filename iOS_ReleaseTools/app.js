var EventProxy=require('eventproxy');

var ep=EventProxy.create("one","two","three",function(one,two,three) {
	console.log(one+three);
});

(function one() {
	console.log("one");
	ep.emit("one","1111");
})();

(function two() {
	console.log("two");
	ep.emit("two");
})();

(function three() {
	console.log("three");
	ep.emit("three","3333");
})();