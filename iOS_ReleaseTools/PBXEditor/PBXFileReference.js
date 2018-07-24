var node_path=require("path");
var fs=require("fs");
var PBXObject=require("./PBXObject").PBXObject;

var PATH_KEY = "path";
var NAME_KEY = "name";
var SOURCETREE_KEY = "sourceTree";
var EXPLICIT_FILE_TYPE_KEY = "explicitFileType";
var LASTKNOWN_FILE_TYPE_KEY = "lastKnownFileType";
var ENCODING_KEY = "fileEncoding";

var trees={
	"ABSOLUTE" : "<absolute>",
	"GROUP" : "<group>",
	"BUILT_PRODUCTS_DIR" : "BUILT_PRODUCTS_DIR",
	"DEVELOPER_DIR" : "DEVELOPER_DIR",
	"SDKROOT" : "SDKROOT",
	"SOURCE_ROOT" : "SOURCE_ROOT"
};

var typeNames={
	 ".a" : "archive.ar" ,
	 ".app" : "wrapper.application" ,
	 ".s" : "sourcecode.asm" ,
	 ".c" : "sourcecode.c.c" ,
	 ".cpp" : "sourcecode.cpp.cpp" ,
	 ".framework" : "wrapper.framework" ,
	 ".h" : "sourcecode.c.h" ,
	 ".pch" : "sourcecode.c.h" ,
	 ".icns" : "image.icns" ,
	 ".m" : "sourcecode.c.objc" ,
	 ".mm" : "sourcecode.cpp.objcpp" ,
	 ".nib" : "wrapper.nib" ,
	 ".storyboard" : "file.storyboard" ,
	 ".plist" : "text.plist.xml" ,
	 ".png" : "image.png" ,
	 ".jpg" : "image.jpeg",
	 ".rtf" : "text.rtf" ,
	 ".tiff" : "image.tiff" ,
	 ".txt" : "text" ,
     ".json" : "text.json",
     ".xml" : "text.xml",
	 ".xcodeproj" : "wrapper.pb-project" ,
	 ".xib" : "file.xib" ,
	 ".strings" : "text.plist.strings" ,
	 ".bundle" : "wrapper.plug-in" ,
	 ".dylib" : "compiled.mach-o.dylib",
     ".tbd":"sourcecode.text-based-dylib-definition",
	".zip" : "archive.zip" 
};

var typePhases = {
	 ".a" : "PBXFrameworksBuildPhase" ,
	 ".app" : null ,
	 ".s" : "PBXSourcesBuildPhase" ,
	 ".c" : "PBXSourcesBuildPhase" ,
	 ".cpp" : "PBXSourcesBuildPhase" ,
	 ".framework" : "PBXFrameworksBuildPhase" ,
	 ".h" : null ,
	 ".pch" : null ,
	 ".icns" : "PBXResourcesBuildPhase" ,
	 ".m" : "PBXSourcesBuildPhase" ,
	 ".mm" : "PBXSourcesBuildPhase" ,
	 ".nib" : "PBXResourcesBuildPhase" ,
	 ".storyboard" : "PBXResourcesBuildPhase" ,
	 ".plist" : "PBXResourcesBuildPhase" ,
	 ".png" : "PBXResourcesBuildPhase" ,
	 ".jpg" : "PBXResourcesBuildPhase" ,
	 ".rtf" : "PBXResourcesBuildPhase" ,
	 ".tiff" : "PBXResourcesBuildPhase" ,
	 ".txt" : "PBXResourcesBuildPhase" ,
     ".json" : "PBXResourcesBuildPhase",
     ".xml" : "PBXResourcesBuildPhase",
	 ".xcodeproj" : null ,
	 ".xib" : "PBXResourcesBuildPhase" ,
	 ".strings" : "PBXResourcesBuildPhase" ,
	 ".bundle" : "PBXResourcesBuildPhase" ,
	 ".dylib" : "PBXFrameworksBuildPhase" ,
     ".tbd" : "PBXFrameworksBuildPhase" ,
	 ".zip" : "PBXResourcesBuildPhase"
};


/**********
 * arg1 guid or filePath
 * arg2 dictionary or tree
 */
function PBXFileReference(arg1,arg2) {	
	this.typeName = "PBXFileReference";
	this.baseName = "PBXObject";
	
	this.buildPhase="";
	
	if(typeof(arg2)=="object") {
		
		this.InitWithGuidAndDic(arg1,arg2);	
		
	} else if(typeof(arg2)=="string") {
		
		this.Init();
		
		this.Add(PATH_KEY,arg1);
		this.Add(NAME_KEY,node_path.basename(arg1));
		
		if(/System\/Library\/Frameworks/.test(arg1)||/usr\/lib/.test(arg1)) {
			this.Add(SOURCETREE_KEY,"SDKROOT");
		}
		else if(fs.existsSync("/"+arg1)) {
			this.Add(SOURCETREE_KEY,trees["ABSOLUTE"]);
		} else {
			this.Add(SOURCETREE_KEY,trees[arg2]);
		}
		
		this.GuessFileType();
	} else {
		this.Init();
	}
};


PBXFileReference.prototype=PBXObject.prototype;

PBXFileReference.prototype.name = function() {
	if(!this.ContainsKey(NAME_KEY)) {
		return null;
	}
	return this._data._data[NAME_KEY];
};

PBXFileReference.prototype.path = function() {
	if(!this.ContainsKey(PATH_KEY)) {
		return null;
	}
	return this._data._data[PATH_KEY];
};

PBXFileReference.prototype.GuessFileType = function() {
	this.Remove(EXPLICIT_FILE_TYPE_KEY);
	this.Remove(LASTKNOWN_FILE_TYPE_KEY);
	var extension=node_path.extname(this._data._data[PATH_KEY]);
	if(!typeNames[extension] && extension !="") { 
		//console.log("Unknown file extension: " + extension + "\nPlease add extension and Xcode type to PBXFileReference.types");
		return ;
	}
	if(extension==""){
		this.Add(LASTKNOWN_FILE_TYPE_KEY,"folder");
		this.buildPhase="PBXResourcesBuildPhase";
	}
	else {
		this.Add(LASTKNOWN_FILE_TYPE_KEY,typeNames[extension]);
		this.buildPhase=typePhases[extension];
	}	
};

PBXFileReference.prototype.SetFileType = function(fileType) {
	this.Remove(EXPLICIT_FILE_TYPE_KEY);
	this.Remove(LASTKNOWN_FILE_TYPE_KEY);
	
	this.Add(EXPLICIT_FILE_TYPE_KEY,fileType);
};

module.exports.PBXFileReference=PBXFileReference;