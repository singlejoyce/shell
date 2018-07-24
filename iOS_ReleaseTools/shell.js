var execFile=require('child_process').execFile,
	child;
var execFileSync = require('child_process').execFileSync;
var fs=require('fs');

function execShell(shellpath,args,msginfo,msgcallback,succcallback) {	
	child=execFile(shellpath,args,{"maxBuffer":5*1024*1024},function(error,stdout,stderr){
		console.log("stdout: "+stdout);
		console.log("stderr: "+stderr);
		msg="";
		if(error !== null) {
			msg="exec error: "+error;
		} else {
			msg=msginfo;
		}		
		msgcallback(msg);
		
		if(typeof(succcallback)=="function") {
			succcallback();
		}		
	});
}

function execShellSync(shellpath,args,msginfo,msgcallback,succcallback) {
	try {
		var stdout = execFileSync(shellpath,args,{"maxBuffer":50*1024*1024,"encoding":"utf8"});
		
		msgcallback(msginfo);

		if(typeof(succcallback)=="function") {
			succcallback();
		}	
		
	} catch (error) {
		msgcallback(error);
		succcallback();
	}

}