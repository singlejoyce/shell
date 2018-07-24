// module.exports.PBXSortedDictionary = function(){};

var XCBuildConfiguration =  require("./../XCBuildConfiguration").XCBuildConfiguration;
var XCConfigurationList = require("./../XCConfigurationList").XCConfigurationList;
var PBXGroup = require("./PBXGroup").PBXGroup;
var PBXFileReference = require("./PBXFileReference").PBXFileReference;
var PBXBuildFile = require("./PBXBuildFile").PBXBuildFile;

function PBXSortedDictionary(objects, typeName) {
	
	this.typeName="PBXSortedDictionary";
	
	this._data = [];
	//[{k:instance},{}...];
	//console.log(objects);
	//console.log(typeName);
	if (typeof(objects) != "undefined" && typeof(typeName) != "undefined") {
		
		for (var k in objects._data) {
			
			if (objects._data[k]._data["isa"] == typeName) {
				
				var _obj = {};	
				var instance={};							
				if(typeName=="XCBuildConfiguration") {					
					instance=new XCBuildConfiguration(k,objects._data[k]);
				} else if(typeName=="PBXGroup") {
					instance=new PBXGroup(k,objects._data[k]);
				} else if(typeName=="PBXBuildFile") {					
					instance=new PBXBuildFile(k,objects._data[k]);					
				} else if(typeName=="PBXFileReference") {
					instance=new PBXFileReference(k,objects._data[k]);
				} else if(typeName=="PBXFrameworksBuildPhase") {
					console.log("PBXFrameworksBuildPhase");
					instance=new PBXFrameworksBuildPhase(k,objects._data[k]);
				} else if(typeName=="XCConfigurationList") {
					instance=new XCConfigurationList(k,objects._data[k]);
				}		
				_obj[k] = instance;				
				this._data.push(_obj);
			}
		}
	}

	// return this._data;
}

PBXSortedDictionary.prototype.Append = function(dictionary) {
	for(var k in dictionary) {
		var obj={};
		obj[k]=dictionary[k];
		this._data.push(obj);
	}
};

PBXSortedDictionary.prototype.ContainsKey = function(key) {
	for(var i=0;i<this._data.length;i++) {
		var obj=this._data[i];
		for(var k in obj) {
			if(k==key) {
				return true;
			}
		}
	}
	return false;
}

/************
 *key value
 */
PBXSortedDictionary.prototype.Add = function(arg1,arg2) { 
	
	var obj=new Object();
	
	if(typeof(arg1)=="string") {
		obj[arg1] = arg2;
	} else {
		obj[arg1.guid()]=arg1;			
	}
	
	this._data.push(obj);
}

module.exports.PBXSortedDictionary = PBXSortedDictionary;

