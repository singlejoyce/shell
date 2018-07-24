var PBXObject=require("./PBXEditor/PBXObject").PBXObject;

function XCConfigurationList(guid,dictionary) {
	this.typeName="XCConfigurationList";
	this.baseName = "PBXObject";
	
	if(typeof(guid)=="undefined") {
		this.Init();
	} else {
		this.InitWithGuidAndDic(guid,dictionary);
	}
}

XCConfigurationList.prototype=PBXObject.prototype;


module.exports.XCConfigurationList=XCConfigurationList;