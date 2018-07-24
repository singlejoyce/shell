var PBXObject = require("./PBXObject").PBXObject;
var PBXDictionary = require("./PBXDictionary").PBXDictionary;

var FILE_REF_KEY = "fileRef";
var SETTINGS_KEY = "settings";
var ATTRIBUTES_KEY = "ATTRIBUTES";
var WEAK_VALUE = "Weak";
var COMPILER_FLAGS_KEY = "COMPILER_FLAGS";

function PBXBuildFile(fileReforguid, weakordictionary) {
	this.typeName = "PBXBuildFile";
	this.baseName = "PBXObject";
	
	if (typeof(fileReforguid) == "string") {
		this.InitWithGuidAndDic(fileReforguid, weakordictionary);
	} else if (typeof(fileReforguid) == "object") {
		this.Init();
		this.Add(FILE_REF_KEY, fileReforguid.guid());
		this.SetWeakLink(weakordictionary);
	} else {
		this.Init();
	}
};

PBXBuildFile.prototype = PBXObject.prototype;

PBXBuildFile.prototype.fileRef = function() {
	return this._data._data[FILE_REF_KEY];
};

PBXBuildFile.prototype.SetWeakLink = function(weak) {
	var setting = null;
	var attributes = null;

	if (!this.ContainsKey(SETTINGS_KEY)) {
		if (weak) {
			attributes = new PBXList();
			attributes.Add(WEAK_VALUE);

			settings = new PBXDictionary();
			settings.Add(ATTRIBUTES_KEY, attributes);

			this.Add(SETTINGS_KEY, settings);
		}
		return true;
	}

	settings = this._data._data[SETTINGS_KEY];
	if (!settings.ContainsKey(ATTRIBUTES_KEY)) {
		if (weak) {
			attributes = new PBXList();
			attributes.Add(WEAK_VALUE);
			settings.Add(ATTRIBUTES_KEY, attributes);
			return true;
		} else {
			return false;
		}
	} else {
		attributes = settings._data[ATTRIBUTES_KEY];
	}

	if (weak) {
		attributes.Add(WEAK_VALUE);
	} else {
		attributes.Remove(WEAK_VALUE);
	}

	settings.Add(ATTRIBUTES_KEY, attributes);
	this.Add(SETTINGS_KEY, settings);

	return true;
};

PBXBuildFile.prototype.AddCompilerFlag = function(flag) {
	if (!this._data.ContainsKey(SETTINGS_KEY)) {
		this._data._data[SETTINGS_KEY] = new PBXDictionary();
	}

	if (!this._data._data[SETTINGS_KEY].ContainsKey(COMPILER_FLAGS_KEY)) {
		this._data._data[SETTINGS_KEY].Add(COMPILER_FLAGS_KEY, flag);
		return true;
	}

	var flags = this._data._data[SETTINGS_KEY]._data[COMPILER_FLAGS_KEY].split(' ');
	for (var i = 0; i < flags.length; i++) {
		if (flags[i] == flag) {
			return false;
		}
	}

	this._data._data[SETTINGS_KEY]._data[COMPILER_FLAGS_KEY] = flags.join(" ") + " " + flag;
	return true;
};

module.exports.PBXBuildFile = PBXBuildFile;
