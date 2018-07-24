var APPPATH="/Users/demon/Documents/diandianle/IOS自动化工具";
var UINTYPATH="/Applications/Unity/Unity.app/Contents/MacOS/Unity";

var BuildArgsConfig={
	"BUILDIPASHELLPATH":APPPATH+"/buildipa",
	"COPYSHELLPATH":APPPATH+"/copyshell",
	"UNITYBUILDSHELLPATH":APPPATH+"/unitybuildshell",
	"UPDATESHELLPATH":APPPATH+"/update",
	"BUILDRESSHELLPATH":APPPATH+"/build",
	"CONFIGSHELLPATH":APPPATH+"/configshell",
	"PUBLISHSHELLPATH":APPPATH+"/publish-x",
	"DATAPATH":APPPATH+"/data"
};

function PlatformConfig() {
	this.unitypath="";
	this.name="";
	this.bundleid="";
	this.apkversion="";
	this.customversion="";
	this.bundleversion="";
	this.premacro="";
	this.mods=[];
	this.inhoursesign="";
	this.devsign="";
	this.dissign="";
	this.urlschemes=[];
	this.otherfiles=[];
	this.appcontrollerpath="";
	this.inhourseprofile="";
	this.devprofile="";
	this.pubprofile="";
	this.isselected=false;
	this.buildallipa=true;
}

function ProjectSetting()
{
	this.projectsvnpath="";
	this.configsvnpath="";
	this.ressvnpaths=[];
}

function Mod() {
	this.group="";
	this.libs=[];
	this.frameworks=[];
	this.headerpaths=[];
	this.files=[];
	this.folders=[];
	this.excludes=["^.*.meta$", "^.*.mdown$", "^.*.pdf$",".DS_Store"];
	this.linker_flags=[];
	this.compiler_flags=[];
}