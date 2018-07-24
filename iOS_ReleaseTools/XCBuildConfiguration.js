module.exports.XCBuildConfiguration=XCBuildConfiguration;

var PBXObject = require("./PBXEditor/PBXObject").PBXObject;
var PBXList= require("./PBXEditor/PBXList").PBXList;

var PBXDictionary = require("./PBXEditor/PBXDictionary").PBXDictionary;
var PBXSortedDictionary = require("./PBXEditor/PBXSortedDictionary").PBXSortedDictionary;

var BUILDSETTINGS_KEY = "buildSettings";
var HEADER_SEARCH_PATHS_KEY = "HEADER_SEARCH_PATHS";
var LIBRARY_SEARCH_PATHS_KEY = "LIBRARY_SEARCH_PATHS";
var FRAMEWORK_SEARCH_PATHS_KEY = "FRAMEWORK_SEARCH_PATHS";
var OTHER_C_FLAGS_KEY = "OTHER_CFLAGS";
var OTHER_LDFLAGS_KEY = "OTHER_LDFLAGS";

function XCBuildConfiguration(guid,dictionary) {
	this.typeName = "XCBuildConfiguration";
	this.baseName = "PBXObject";
	if(typeof(guid)!="undefined") {
		this.InitWithGuidAndDic(guid,dictionary);
	} else {		
		this.Init();
	}
	
}

XCBuildConfiguration.prototype=PBXObject.prototype;

XCBuildConfiguration.prototype.buildSettings = function () {
	if(this.ContainsKey(BUILDSETTINGS_KEY)) {
		if(this._data._data[BUILDSETTINGS_KEY].typeName==="PBXDictionary") {
			var ret = new PBXSortedDictionary();
			ret.Append(this._data._data[BUILDSETTINGS_KEY]);				
			return ret;
		}		
		return this._data._data[BUILDSETTINGS_KEY];
	}
	return null;
};

XCBuildConfiguration.prototype.AddSearchPaths = function(path,key,recursive,quoted) {
	
	var modified=false;
	var paths=path;
	if(typeof(paths)=="string") {
		paths=new PBXList(paths);
	}
	if(typeof(recursive)=="undefined") {
		recursive=true;
	}
	if(typeof(quoted)=="undefined") {
		quoted=false;
	}
	
	if(!this.ContainsKey(BUILDSETTINGS_KEY)) {
		
		var dic=new PBXDictionary();
		
		this.Add(BUILDSETTINGS_KEY,dic);//PBXSortedDictionary
	}	
	
	for(var i=0;i<paths._data.length;i++) {
		
		var currentPath=paths._data[i];
		
		if(!this._data._data[BUILDSETTINGS_KEY].ContainsKey(key)) {
			this._data._data[BUILDSETTINGS_KEY].Add(key,new PBXList());
		} else if(typeof(this._data._data[BUILDSETTINGS_KEY]._data[key])=="string") {
			var list=new PBXList();
			list.Add(this._data._data[BUILDSETTINGS_KEY]._data[key]);
			this._data._data[BUILDSETTINGS_KEY]._data[key]=list;
		}
		
		if(currentPath.match(/\s/)) {
			quoted=true;
		}
		
		if(quoted) {
			if(currentPath.length>3 && currentPath.substring(currentPath.length-3)=="/**") {
				currentPath ='\\\"' + currentPath.replace('/**','\\\"/**');
			} else {
				currentPath = '\\\"' + currentPath + '\\\"';
			}				 
		}
	
		if(!this._data._data[BUILDSETTINGS_KEY]._data[key].Contains(currentPath)) {
			this._data._data[BUILDSETTINGS_KEY]._data[key].Add(currentPath);				
			modified=true;
		}
	}
	
	return modified;
};

XCBuildConfiguration.prototype.AddHeaderSearchPaths = function (paths,recursive) {
	if(typeof(recursive)=="undefined") {
		recursive=true;
	}
	
	return this.AddSearchPaths(paths,HEADER_SEARCH_PATHS_KEY,recursive);
};

XCBuildConfiguration.prototype.AddLibrarySearchPaths = function(paths,recursive) {
	if(typeof(recursive)=="undefined") {
		recursive=true;
	}
	return this.AddSearchPaths(paths,LIBRARY_SEARCH_PATHS_KEY,recursive);
};

XCBuildConfiguration.prototype.AddFrameworkSearchPaths = function (paths,recursive) {
	if(typeof(recursive)=="undefined") {
		recursive=true;
	}
	return this.AddSearchPaths(paths,FRAMEWORK_SEARCH_PATHS_KEY,recursive);
};

XCBuildConfiguration.prototype.AddOtherCFlags = function (flag) {
	var modified=false;
	var flags=flag;
	
	if(typeof(flags)=="string") {
		flags = new PBXList(flags);
	} 
	
	if(!this.ContainsKey(BUILDSETTINGS_KEY)) {
		this.Add(BUILDSETTINGS_KEY,new PBXDictionary());
	}
	
	for(var i=0;i<flags._data.length;i++) {
		
		if(!this._data._data[BUILDSETTINGS_KEY].ContainsKey(OTHER_C_FLAGS_KEY)) {
			this._data._data[BUILDSETTINGS_KEY].Add(OTHER_C_FLAGS_KEY,new PBXList());
		} else if(typeof(this._data._data[BUILDSETTINGS_KEY]._data[OTHER_C_FLAGS_KEY])=="string") {
			var tempString=this._data._data[BUILDSETTINGS_KEY]._data[OTHER_C_FLAGS_KEY];
			this._data._data[BUILDSETTINGS_KEY]._data[OTHER_C_FLAGS_KEY]=new PBXList();
			this._data._data[BUILDSETTINGS_KEY]._data[OTHER_C_FLAGS_KEY].Add(tempString);
		}
		
		if(!this._data._data[BUILDSETTINGS_KEY]._data[OTHER_C_FLAGS_KEY].Contains(flags._data[i])) {
			this._data._data[BUILDSETTINGS_KEY]._data[OTHER_C_FLAGS_KEY].Add(flags._data[i]);
			modified=true;
		}			
	}
	return modified;
};

XCBuildConfiguration.prototype.AddOtherLinkerFlags = function (flag) {
	var modified=false;
	var flags=flag;
	
	if(typeof(flags)=="string") {
		flags = new PBXList(flags);
	} 
	
	if(!this.ContainsKey(BUILDSETTINGS_KEY)) {
		this.Add(BUILDSETTINGS_KEY,new PBXSortedDictionary());
	}
	
	for(var i=0;i<flags._data.length;i++) {
		if(!this._data._data[BUILDSETTINGS_KEY].ContainsKey(OTHER_LDFLAGS_KEY)) {
			this._data._data[BUILDSETTINGS_KEY].Add(OTHER_LDFLAGS_KEY,new PBXList());
		} else if(typeof(this._data._data[BUILDSETTINGS_KEY]._data[OTHER_LDFLAGS_KEY])=="string") {
			var tempString=this._data._data[BUILDSETTINGS_KEY]._data[OTHER_LDFLAGS_KEY];
			this._data._data[BUILDSETTINGS_KEY]._data[OTHER_LDFLAGS_KEY]=new PBXList();
			if(tempString!="") {
				this._data._data[BUILDSETTINGS_KEY]._data[OTHER_LDFLAGS_KEY].Add(tempString);
			}				
		}
		
		if(!this._data._data[BUILDSETTINGS_KEY]._data[OTHER_LDFLAGS_KEY].Contains(flags._data[i])) {
			this._data._data[BUILDSETTINGS_KEY]._data[OTHER_LDFLAGS_KEY].Add(flags._data[i]);
			modified=true;
		}
	}
	return modified;
};

XCBuildConfiguration.prototype.overwriteBuildSetting = function (settingName,settingValue,isstring) {
	
	if(typeof(isstring)=="undefined") {
		isstring=false;
	}

	var modified=false;
	
	if(!this.ContainsKey(BUILDSETTINGS_KEY)) {
		this.Add(BUILDSETTINGS_KEY,new PBXSortedDictionary());
	}
	
	if(!this._data._data[BUILDSETTINGS_KEY].ContainsKey(settingName)) {
		this._data._data[BUILDSETTINGS_KEY].Add(settingName,new PBXList());
	} else if (typeof(this._data._data[BUILDSETTINGS_KEY]._data[settingName])=="string") {
		this._data._data[BUILDSETTINGS_KEY]._data[settingName]=new PBXList();
	} 
	
	if(!this._data._data[BUILDSETTINGS_KEY]._data[settingName].Contains(settingValue)) {
		this._data._data[BUILDSETTINGS_KEY]._data[settingName].Add(settingValue);
		modified=true;
	}
	
	if(typeof(isstring)!="undefined" && isstring) {
		this._data._data[BUILDSETTINGS_KEY]._data[settingName]=settingValue;
	}
	return modified;
};
