var PBXObject = require("./PBXObject").PBXObject;
var PBXList = require("./PBXList").PBXList;

var FILES_KEY = "files";

function PBXBuildPhase(guid, dictionary) {
	this.typeName="PBXBuildPhase";
	this.baseName = "PBXObject";
	
	if(typeof(guid)=="undefined" || guid ==null) {
		this.Init();
	} else {
		this.InitWithGuidAndDic(guid,dictionary);
	}
};

PBXBuildPhase.prototype=PBXObject.prototype;

PBXBuildPhase.prototype.AddBuildFile = function(file) {
	if(!this.ContainsKey(FILES_KEY)) {
		this.Add(FILES_KEY,new PBXList());
	}
	this._data._data[FILES_KEY].Add(file.guid());
	return true;
};

PBXBuildPhase.prototype.RemoveBuildFile = function(id) {
	if( !this.ContainsKey( FILES_KEY ) ) {
		this.Add( FILES_KEY, new PBXList() );
		return;
	}
	
	this._data._data[FILES_KEY].Remove( id );
};

PBXBuildPhase.prototype.HasBuildFile = function(id)
{
	if( !this.ContainsKey( FILES_KEY ) ) {
		this.Add( FILES_KEY, new PBXList() );
		return false;
	}

	if( !this.IsGuid( id ) )
		return false;

	return this._data._data[ FILES_KEY ].Contains( id );
};

PBXBuildPhase.prototype.files = function() {				
	if( !this.ContainsKey( FILES_KEY ) ) {
		this.Add( FILES_KEY, new PBXList() );
	}
	return this._data._data[ FILES_KEY ];
};

module.exports.PBXBuildPhase = PBXBuildPhase;

function PBXFrameworksBuildPhase(guid,dictionary) {
	this.typeName="PBXFrameworksBuildPhase";
	this.baseName="PBXBuildPhase";
	if(typeof(guid)=="undefined" || guid==null) {
		this.Init();
	} else {
		this.InitWithGuidAndDic(guid,dictionary);
	}
};

PBXFrameworksBuildPhase.prototype=PBXBuildPhase.prototype;


module.exports.PBXFrameworksBuildPhase = PBXFrameworksBuildPhase;

function PBXResourcesBuildPhase(guid,dictionary) {
	this.typeName="PBXResourcesBuildPhase";
	this.baseName="PBXBuildPhase";
	
	if(typeof(guid)=="undefined" || guid==null) {
		this.Init();
	} else {
		this.InitWithGuidAndDic(guid,dictionary);
	}
};

PBXResourcesBuildPhase.prototype=PBXBuildPhase.prototype;

module.exports.PBXResourcesBuildPhase = PBXResourcesBuildPhase;


function PBXShellScriptBuildPhase(guid,dictionary) {
	this.typeName="PBXShellScriptBuildPhase";
	this.baseName="PBXBuildPhase";
	if(typeof(guid)=="undefined" || guid==null) {
		this.Init();
	} else {
		this.InitWithGuidAndDic(guid,dictionary);
	}
};

PBXShellScriptBuildPhase.prototype=PBXBuildPhase.prototype;


module.exports.PBXShellScriptBuildPhase = PBXShellScriptBuildPhase;


function PBXSourcesBuildPhase(guid,dictionary) {
	this.typeName="PBXSourcesBuildPhase";
	this.baseName="PBXBuildPhase";
	if(typeof(guid)=="undefined" || guid==null) {
		this.Init();
	} else {
		this.InitWithGuidAndDic(guid,dictionary);
	}
};

PBXSourcesBuildPhase.prototype=PBXBuildPhase.prototype;

module.exports.PBXSourcesBuildPhase = PBXSourcesBuildPhase;


function PBXCopyFilesBuildPhase(guid,dictionary) {
	this.typeName="PBXCopyFilesBuildPhase";
	this.baseName="PBXBuildPhase";
	if(typeof(guid)=="undefined" || guid==null) {
		this.Init();
	} else {
		this.InitWithGuidAndDic(guid,dictionary);
	}
};

PBXCopyFilesBuildPhase.prototype=PBXBuildPhase.prototype;

module.exports.PBXCopyFilesBuildPhase = PBXCopyFilesBuildPhase;








