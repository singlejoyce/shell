var PBXObject=require('./PBXObject').PBXObject;

var MAINGROUP_KEY="mainGroup";

function PBXProject(guid,dictionary) {
	
	this.typeName="PBXProject";
	this.baseName="PBXObject";
	
	if(typeof(guid)=="undefined") {
		this.Init();
	} else {
		this.InitWithGuidAndDic(guid,dictionary);
	}	
};

PBXProject.prototype=PBXObject.prototype;

PBXObject.prototype.mainGroupID = function() {
	return this._data._data[MAINGROUP_KEY];
};

module.exports.PBXProject=PBXProject;