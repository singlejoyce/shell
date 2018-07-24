var PBXObject = require("./PBXObject").PBXObject;
var PBXList = require("./PBXList").PBXList;

var NAME_KEY = "name";
var CHILDREN_KEY = "children";
var PATH_KEY = "path";
var SOURCETREE_KEY = "sourceTree";


/**********
 * arg1 name or guid
 * arg2 path or dictionary
 * arg3 tree 
 */
function PBXGroup(arg1, arg2, arg3) { 
	
	this.typeName = "PBXGroup";
	this.baseName = "PBXObject";
	
	if(typeof(arg2)!="undefined" && arg2 !=null && typeof(arg2)=="object") {
		
		this.InitWithGuidAndDic(arg1,arg2);
		
	} else if(typeof(arg1)!="undefined" && typeof(arg3)=="undefined") {
				
		arg3="SOURCE_ROOT";
		
		this.Init();
		
		this.Add(CHILDREN_KEY, new PBXList());
		this.Add(NAME_KEY, arg1);
		
		if (arg2 != null) {
			this.Add(PATH_KEY, arg2);
			this.Add(SOURCETREE_KEY, arg3);

		} else {
			this.Add(SOURCETREE_KEY, "<group>");
		}	
	} else {
		this.Init();
	}	
};

PBXGroup.prototype = PBXObject.prototype;

PBXGroup.prototype.children = function() {
	if (!this.ContainsKey(CHILDREN_KEY)) {
		this.Add(CHILDREN_KEY, new PBXList());
	}
	return this._data._data[CHILDREN_KEY]; //(PBXList)
};

PBXGroup.prototype.name = function() {
	if (!this.ContainsKey(NAME_KEY)) {
		return null
	}
	return this._data._data[NAME_KEY];
};

PBXGroup.prototype.path = function() {
	if (!this.ContainsKey(PATH_KEY)) {
		return null;
	}
	return this._data._data[PATH_KEY];
};

PBXGroup.prototype.sourceTree = function() {
	return this._data._data[SOURCETREE_KEY];
};

PBXGroup.prototype.AddChild = function(child) {
	if (child.typeName == "PBXFileReference" || child.typeName == "PBXGroup") {
		//console.log(this.children());
		this.children()._data.push(child.guid());
		return child.guid();
	}
	return null;
};

PBXGroup.prototype.RemoveChild = function(id) {
	if (!this.IsGuid(id)) {
		return;
	}
	var c = children();
	var i;
	for (i = 0; i < c.length; i++) {
		if (c[i] == id) {
			break;
		}
	}
	c.slice(0, i).concat(c.slice(i + 1));
};

PBXGroup.prototype.HasChild = function(id) {
	if (!this.ContainsKey(CHILDREN_KEY)) {
		this.Add(CHILDREN_KEY, new PBXList());
		return false;
	}
	if (!this.IsGuid(id)) {
		return false;
	}

	return this._data._data[CHILDREN_KEY].Contains(id); //PBXList;
};

PBXGroup.prototype.GetName = function() {
	return this._data._data[NAME_KEY];
};

module.exports.PBXGroup = PBXGroup;
