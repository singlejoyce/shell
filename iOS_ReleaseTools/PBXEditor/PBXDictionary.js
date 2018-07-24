module.exports.PBXDictionary = PBXDictionary;

var PBXNativeTarget=require("./PBXObject").PBXNativeTarget;
var XCBuildConfiguration=require("./../XCBuildConfiguration").XCBuildConfiguration;
var XCConfigurationList=require("./../XCConfigurationList").XCConfigurationList;
var PBXFrameworksBuildPhase=require("./PBXBuildPhase").PBXFrameworksBuildPhase;
var PBXResourcesBuildPhase=require("./PBXBuildPhase").PBXResourcesBuildPhase;
var PBXShellScriptBuildPhase=require("./PBXBuildPhase").PBXShellScriptBuildPhase;
var PBXCopyFilesBuildPhase=require("./PBXBuildPhase").PBXCopyFilesBuildPhase;
var PBXSourcesBuildPhase=require("./PBXBuildPhase").PBXSourcesBuildPhase;

function PBXDictionary(objects,_typeName) { 	
	this.typeName = "PBXDictionary";
	this._data = {};
	
	if(typeof(objects)=="undefined") {
		
		
	} else if(typeof(objects)=="object") { 
		if(objects.typeName=="PBXDictionary") {
			for(var k in objects._data) {
				if(objects._data[k]._data["isa"]==_typeName) {
					switch(_typeName) {
						case "PBXNativeTarget" :
							var instance=new PBXNativeTarget(k,objects._data[k]);
							this.Add(k,instance);
							break;
						case "XCBuildConfiguration" :
							var instance=new XCBuildConfiguration(k,objects._data[k]);
							this.Add(k,instance);
							break;
						case "XCConfigurationList" :
							var instance=new XCConfigurationList(k,objects._data[k]);
							this.Add(k,instance);
							break;
						case "PBXFrameworksBuildPhase" :
							var instance=new PBXFrameworksBuildPhase(k,objects._data[k]);
							this.Add(k,instance);
							break;
						case "PBXResourcesBuildPhase" :
							var instance=new PBXResourcesBuildPhase(k,objects._data[k]);
							this.Add(k,instance);
							break;
						case "PBXShellScriptBuildPhase" :
							// console.log(k);
							var instance=new PBXShellScriptBuildPhase(k,objects._data[k]);
							this.Add(k,instance);
							// console.log(instance);
							break;
						case "PBXSourcesBuildPhase" :
							var instance=new PBXSourcesBuildPhase(k,objects._data[k]);
							this.Add(k,instance);
							break;
						case "PBXCopyFilesBuildPhase" :
							var instance=new PBXCopyFilesBuildPhase(k,objects._data[k]);
							this.Add(k,instance);
							break;
					}
				}
			}
		} 
	}
}

PBXDictionary.prototype.Append = function(dictionary) {
	if(dictionary.typeName=="PBXSortedDictionary") {
		for(var i=0;i<dictionary._data.length;i++) {
			var obj=dictionary._data[i];
			for(var k in obj) {
				this._data[k]=obj[k];
			}
		}
	} else {
		for(var k in dictionary._data) {
			this._data[k]=dictionary._data[k];
		}		
	}	
};

PBXDictionary.prototype.AppendSorted = function(dictionary) {
	for(var i=0;i<dictionary._data.length;i++) {
		var obj=dictionary._data[i];
		for(var k in dictionary._data[i]) {
			this._data[k]=dictionary._data[i][k];
		}
	}
};

PBXDictionary.prototype.ToCSV = function() {
	var ret = "";
	for(var k in this._data) {
		ret += "<";
		ret += k;
		ret += ", ";
		ret += this._data[k];
		ret += ">, ";			
	}
	return ret;
};

PBXDictionary.prototype.ToString = function() {
	return "{"+this.ToCSV()+"}";
};

PBXDictionary.prototype.ContainsKey = function(key) {
	for (var s in this._data) {
		 //console.log(s);
		if (s === key) {
			return true;
		}
	}
	return false;
};

PBXDictionary.prototype.Remove = function(key) {
	return delete this._data[key]; 
};

PBXDictionary.prototype.Add = function(keyornewObject, value) { 
	if(typeof(keyornewObject)=="object") {
		this._data(keyornewObject.guid,keyornewObject);
	} else {
		this._data[keyornewObject] = value;
	}
};

module.exports.PBXDictionary = PBXDictionary;
