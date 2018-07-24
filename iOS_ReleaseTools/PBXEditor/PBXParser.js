var fs = require('fs');
var PBXDictionary = require("./PBXDictionary").PBXDictionary;
var PBXList=require("./PBXList").PBXList;

var PBX_HEADER_TOKEN = "// !$*UTF8*$!\n";
var WHITESPACE_SPACE = ' ';
var WHITESPACE_TAB = '\t';
var WHITESPACE_NEWLINE = '\n';
var WHITESPACE_CARRIAGE_RETURN = '\r';
var ARRAY_BEGIN_TOKEN = '(';
var ARRAY_END_TOKEN = ')';
var ARRAY_ITEM_DELIMITER_TOKEN = ',';
var DICTIONARY_BEGIN_TOKEN = '{';
var DICTIONARY_END_TOKEN = '}';
var DICTIONARY_ASSIGN_TOKEN = '=';
var DICTIONARY_ITEM_DELIMITER_TOKEN = ';';
var QUOTEDSTRING_BEGIN_TOKEN = '"';
var QUOTEDSTRING_END_TOKEN = '"';
var QUOTEDSTRING_ESCAPE_TOKEN = '\\';
var END_OF_FILE = 0x1A; // (char)0x1A;
var COMMENT_BEGIN_TOKEN = "/*";
var COMMENT_END_TOKEN = "*/";
var COMMENT_LINE_TOKEN = "//";
var BUILDER_CAPACITY = 20000;

function PBXResolver(pbxData) {
	
	this.objects = pbxData._data["objects"];

	this.rootObject = pbxData["rootObject"];

	this.index=new PBXDictionary();

	this.marker = null;

	this.BuildReverseIndex();	
}

PBXResolver.prototype.ResolveName = function(guid) {
	
	if (!this.objects.ContainsKey(guid)) {
		//console.log(' ResolveName could not resolve ' + guid);
		return "";
	}
	
	var entity = this.objects._data[guid];

	if (entity.typeName == "PBXBuildFile" || entity.baseName == "PBXBuildFile") { 
		// console.log("---ResolveName---");
// 		console.log(entity._data);
		return this.ResolveName(entity.fileRef());
	} else if (entity.typeName == "PBXFileReference" || entity.baseName == "PBXFileReference") {
		var casted = entity;
		return casted.name() != null ? casted.name() : casted.path();
	} else if (entity.typeName == "PBXGroup" || entity.baseName == "PBXGroup") {
		var casted = entity;
		return casted.name() != null ? casted.name() : casted.path();
	} else if (entity.typeName == "PBXProject" || guid == this.rootObject) {
		return "Project object";
	} else if (entity.typeName == "PBXFrameworksBuildPhase") {
		return "Frameworks";
	} else if (entity.typeName == "PBXResourcesBuildPhase") {
		return "Resources";
	} else if (entity.typeName == "PBXShellScriptBuildPhase") {
		return "ShellScript";
	} else if (entity.typeName == "PBXSourcesBuildPhase") {
		return "Sources";
	} else if (entity.typeName == "PBXCopyFilesBuildPhase") {
		return "CopyFiles";
	} else if (entity.typeName == "XCConfigurationList") {
		var casted = entity;
		if (casted.ContainsKey("defaultConfigurationName")) {
			return casted._data._data["defaultConfigurationName"];
		}
		return null
	} else if (entity.typeName == "PBXNativeTarget") {
		var obj = entity;

		if (obj.ContainsKey("name")) {
			return obj._data._data["name"];
		}

		return null;
	} else if (entity.typeName == "XCBuildConfiguration") {
		var obj = entity;

		if (obj.ContainsKey("name")) {
			return obj._data._data["name"];
		}
	} else if (entity.baseName=="PBXObject") {
		var obj = entity;

		if (obj.ContainsKey("name")) {
			return obj._data._data["name"];
		}
	}

	//console.log("UNRESOLVED GUID:" + guid);
	return null;
};

PBXResolver.prototype.ResolveBuildPhaseNameForFile = function(guid) {
	
	if(this.objects.ContainsKey(guid)) {
		var obj = this.objects._data[guid];
		if(obj.baseName == "PBXObject") {
			var entity = obj;
			
			if(this.index.ContainsKey(entity.guid())) {
				var parent_guid = this.index._data[entity.guid()];
				
				if(this.objects.ContainsKey(parent_guid)) {
					var parent = this.objects._datta[parent_guid];
					
					if(parent.baseName == "PBXBuildPhase") {
						var ret = this.ResolveName(parent.guid());
						return ret;
					}
				}
			}
		}
		
	}
	return null;
};

PBXResolver.prototype.BuildReverseIndex = function() {

	for (var k in this.objects._data) {
		
		if(this.objects._data[k].baseName=="PBXBuildPhase") {
			
			for (var a = 0; a < this.objects._data[k].files()._data.length; a++) {
				this.index[this.objects._data[k].files()._data[a]] = k;
			}
			
				
		} else if (this.objects._data[k].typeName == 'PBXGroup') {

			for (var a = 0; a < this.objects._data[k].children()._data.length; a++) {
				this.index[this.objects._data[k].children()._data[a]] = k;
			}
			
		}

	}
}


function PBXParser() {
	this.data = [];
	this.index = 0;
	this.resolver;
	this.builder = "";
}

PBXParser.prototype = {

	Decode: function(_data) {

		// TODO data.StartsWith(PBX_HEADER_TOKEN)

		// var re=/^[PBX_HEADER_TOKEN]/;
		// //console.log(_data.match(re));

		_data = _data.substring(13);
		this.data = _data.split("");
		this.index = 0;
		
		var s = this.ParseValue();
		
		return s;
	},

	Indent: function(builder, deep) {
		for (var i = 0; i < deep; i++) {
			this.builder += '\t';
		}
	},

	Endline: function(builder, useSpace) {
		if (typeof(useSpace) == "undefined") {
			useSpace = false;
		}
		this.builder += useSpace ? " " : "\n";
	},

	MarkSection: function(builder, name) {
				
		if (typeof(name) == "undefined") {
			name = null;
		}
		if (this.marker == null && name == null) {
			return;
		}

		if (this.marker != null && name != this.marker) {
			this.builder += '/* End ' + this.marker + ' section */\n';
		}

		if (name != null && name != this.marker) {
			this.builder += '\n/*Begin ' + name + ' Section */\n';
		}

		this.marker = name;
	},

	GUIDComment: function(guid, builder) {
		
		var filename = this.resolver.ResolveName(guid);
		
		var location = this.resolver.ResolveBuildPhaseNameForFile(guid);

		

		if (filename != null) {
			if (location != null) {
				this.builder += ' /* ' + filename + ' in ' + location + ' */'
			} else {
				this.builder += ' /* ' + filename + ' */';
			}
			return true;
		} else {
			//console.log('GUIDComment ' + guid + '[no filename]');
		}

		return false;
	},
	
	SerializeArrary: function(anArray, builder, readable, indent) {
		if (typeof(readable) == "undefined") {
			readable = false;
		}
		if (typeof(indent) == "undefined") {
			indent = 0;
		}

		this.builder += ARRAY_BEGIN_TOKEN;
		if (readable) {
			this.Endline(builder);
		}

		for (var i = 0; i < anArray._data.length; i++) {
			
			var value = anArray._data[i];

			if (readable) {
				this.Indent(builder, indent + 1);
			}

			if (!this.SerializeValue(value, builder, readable, indent + 1)) {
				return false;
			}

			this.builder += ARRAY_ITEM_DELIMITER_TOKEN;

			this.Endline(builder, !readable);
		}

		if (readable) {
			this.Indent(builder, indent);
		}
		this.builder += ARRAY_END_TOKEN;

		return true;
	},
	

	SerializeString: function(aString, builder, useQuotes, readable) {
		
		if (typeof(useQuotes) == "undefined") {
			useQuotes = false;
		}
		if (typeof(readable) == "undefined") {
			readable = false;
		}

		var reg = new RegExp("^[A-Fa-f0-9]{24}$");
		if (aString.match(reg)) {
			this.builder += aString;
			this.GUIDComment(aString, builder);
			return true;
		}

		if (aString == "") {
			this.builder += QUOTEDSTRING_BEGIN_TOKEN;
			this.builder += QUOTEDSTRING_END_TOKEN;
			return true;
		}

		reg = new RegExp("^[A-Za-z0-9_./-]+$");

		if (!aString.match(reg)) {
			useQuotes = true;
		}

		if (useQuotes) {
			this.builder += QUOTEDSTRING_BEGIN_TOKEN;
		}

		this.builder += aString;

		if (useQuotes) {
			this.builder += QUOTEDSTRING_END_TOKEN;
		}

		// console.log(this.builder);
		return true;
	},

	SerializeDictionary: function(dictionary, builder, readable, indent) {

		// console.log(dictionary);
		
		if (typeof(readable) == "undefined") {
			readable = false;
		}
		if (typeof(indent) == "undefined") {
			indent = 0;
		}

		this.builder += DICTIONARY_BEGIN_TOKEN;
		
		if (readable) {
			this.Endline(builder);
		}
		
		for (var k in dictionary._data) {
			
			if(k==="1D60588F0D05DD3D006BFB54") {
				console.log("");
			}
			
			if (readable && indent == 1) {
				this.MarkSection(builder, dictionary._data[k].typeName);
			}

			if (readable) {
				this.Indent(builder, indent + 1);
			}

			this.SerializeString(k, builder, false, readable);

			this.builder += " " + DICTIONARY_ASSIGN_TOKEN + " ";

			this.SerializeValue(dictionary._data[k], builder, (readable &&
				(dictionary._data[k].typeName != "PBXBuildFile") &&
				(dictionary._data[k].typeName != "PBXFileReference")
			), indent + 1);

			this.builder += DICTIONARY_ITEM_DELIMITER_TOKEN;

			this.Endline(builder, !readable);
		}

		if (readable && indent == 1) {
			this.MarkSection(builder, null);
		}

		if (readable) {
			this.Indent(builder, indent);
		}

		this.builder += DICTIONARY_END_TOKEN;

		return true;

	},
	SerializeSortedDictionary: function(dictionary, builder, readable, indent) {
		if (typeof(readable) == "undefined") {
			readable = false;
		}
		if (typeof(indent) == "undefined") {
			indent = 0;
		}

		this.builder += DICTIONARY_BEGIN_TOKEN;
		if (readable) {
			this.Endline(builder);
		}
		
		 for(var i=0;i<dictionary._data.length;i++) {
			 var obj=dictionary._data[i];
			 for(var ok in obj) {
	 			if (readable && indent == 1) {
	 				this.MarkSection(builder, obj[ok].typeName);
	 			}

	 			if (readable) {
	 				this.Indent(builder, indent + 1);
	 			}

	 			this.SerializeString(ok, builder, false, readable);

	 			this.builder += " " + DICTIONARY_ASSIGN_TOKEN + " ";

	 			this.SerializeValue(obj[ok]._data, builder, (readable &&
	 				obj[ok].typeName != "PBXBuildFile" &&
	 				obj[ok].typeName != "PBXFileReference"
	 			), indent + 1);

	 			this.builder += DICTIONARY_ITEM_DELIMITER_TOKEN;

	 			this.Endline(builder, !readable);
	 		}			 
		 }	
		

		if (readable && indent == 1) {
			this.MarkSection(builder, null);
		}

		if (readable) {
			this.Indent(builder, indent);
		}

		this.builder += DICTIONARY_END_TOKEN;

		return true;

	},

SerializeValue: function(value, builder, readable, indent) {
	
	if (typeof(readable) == "undefined") {
		readable = false;
	}
	if (typeof(indent) == "undefined") {
		indent = 0;
	}
	
	// console.log(value);
	
	if (typeof(value) == "undefined" || value == null) {
		this.builder += "null";
	} else if (value.baseName == "PBXObject" || value.typeName == "PBXObject") {
		this.SerializeDictionary(value._data, builder, readable, indent);
	} else if (value.typeName == "PBXDictionary" || value.baseName=="PBXDictionary") { 
		this.SerializeDictionary(value, builder, readable, indent);
	} else if(value.typeName=="PBXSortedDictionary") {
		this.SerializeSortedDictionary(value, builder, readable, indent);
	} else if (value.typeName == "PBXList" || value.baseName == "PBXList") {
		this.SerializeArrary(value, builder, readable, indent);
	} else if (typeof(value) == "string") {
		this.SerializeString(value, builder, false, readable);
	} else if (typeof(value) == "boolean") {
		if (value) {
			this.builder += "1";
		} else {
			this.builder += "0";
		}
		//} else if()//IsPrimitive;
	} else if(typeof(value)=="number") {
		this.builder += value;
	} else if(typeof(value._data)!="undefined") {
		this.SerializeValue(value._data,builder,readable,indent);
	}
	else {
		//console.log("Error: unknown object of type " + typeof(value));
		return false;
	}

	return true;
},

Encode: function(pbxData, readable) {
	if (typeof(readable) == "undefined") {
		readable = false;
	}

	this.resolver = new PBXResolver(pbxData);
	this.builder += PBX_HEADER_TOKEN;

	var success = this.SerializeValue(pbxData, {}, readable);
	
	this.resolver = null;
	
	this.builder += "\n";

	return (success ? this.builder :null);
},

StepForeward: function(step) {
	if(typeof(step)=="undefined") {
		step = 1;
	}
	this.index = Math.min(this.data.length, this.index + step); // Math.min(data.length,index+step);		
	return this.data[this.index] + "";
},

StepBackward: function(step) {
	if(typeof(step)=="undefined") {
		step = 1;
	}
	this.index = Math.max(0, this.index - step);
	return this.data[this.index] + "";
},

Peek: function(step) {
	if(typeof(step)=="undefined") {
		step = 1;
	}
	var sneak = "";
	for (var i = 1; i <= step; i++) {
		if (this.data.length - 1 < this.index + i) {
			break;
		}
		sneak += this.data[this.index + i];
	}
	return sneak + "";
},

SkipComments: function() {
	var s = "";
	var tag = this.Peek(2);
	switch (tag) {
		case COMMENT_BEGIN_TOKEN:
			{
				while (this.Peek(2) != COMMENT_END_TOKEN) {
					s += this.StepForeward();
				}
				s += this.StepForeward(2);
				break;
			}
		case COMMENT_LINE_TOKEN:
			{
				while (!this.StepForeward().match(/\n/)) {
					continue;
				}
				break;
			}
		default:
			return false;
	}
	return true;
},

SkipWhitespaces: function() {
	var whitespace = false;
	while (this.StepForeward(1).match(/\s/)) {
		whitespace = true;
	}

	this.StepBackward();

	if (this.SkipComments()) {
		whitespace = true;
		this.SkipWhitespaces();
	}

	return whitespace;
},

NextToken: function() {
	this.SkipWhitespaces();
	var nt = this.StepForeward();
	return nt;
},

ParseDictionary: function() {

	this.SkipWhitespaces();
	var dictionary = new PBXDictionary();
	var keyString = "";
	var valueObject = null;

	var complete = false;
	while (!complete) {
		if (this.data.length == (this.index + 1)) {
			Console.log("Error: reached end of file inside a dictionary: " + index);
			complete = true;
		} else {
			var nt = this.NextToken();
			switch (nt) {
				case END_OF_FILE:
					Console.log("Error: reached end of file inside a dictionary: " + index);
					complete = true;
					break;
				case DICTIONARY_ITEM_DELIMITER_TOKEN:
					keyString = "";
					valueObject = null;
					break;
				case DICTIONARY_END_TOKEN:
					keyString = "";
					valueObject = null;
					complete = true;
					break;
				case DICTIONARY_ASSIGN_TOKEN:
					valueObject = this.ParseValue();
					if (!dictionary.ContainsKey(keyString)) {
						dictionary.Add(keyString, valueObject);
					}
					break;
				default:
					this.StepBackward();
					keyString = this.ParseValue();
					break;
			}
		}
	}

	return dictionary;
},

ParseArray: function() {
	var list =new PBXList();
	var complete = false;
	while (!complete) {
		if (this.data.length == (this.index + 1)) {
			complete = true;
		} else {
			switch (this.NextToken()) {
				case END_OF_FILE:
					Console.log("Error: Reached end of file inside a list: " + list);
					complete = true;
					break;
				case ARRAY_END_TOKEN:
					complete = true;
					break;
				case ARRAY_ITEM_DELIMITER_TOKEN:
					break;
				default:
					this.StepBackward();
					list._data.push(this.ParseValue());
					break;
			}
		}
	}
	return list;
},

ParseString: function() {
	var s = "";
	var c = this.StepForeward();
	while (c != QUOTEDSTRING_END_TOKEN) {
		s += c;

		if (c == QUOTEDSTRING_ESCAPE_TOKEN) {
			s += this.StepForeward();
		}

		c = this.StepForeward();
	}
	return s;
},

ParseEntity: function() {
	var word = "";

	while (!this.Peek().match(/[;,\s=]/)) {
		word += this.StepForeward();
	}

	if (word.length != 24 && word.match(/^\d+$/)) {
		return word;
	}

	return word;
},

ParseValue: function() {
	if (this.data.length == (this.index + 1)) {
		Console.log("End of file");
		return null;
	} else {
		var nt = this.NextToken();
		switch (nt) {
			case END_OF_FILE:
				Console.log("End of file");
				return null;
			case DICTIONARY_BEGIN_TOKEN:
				return this.ParseDictionary();
			case ARRAY_BEGIN_TOKEN:
				return this.ParseArray();
			case QUOTEDSTRING_BEGIN_TOKEN:
				return this.ParseString();
			default:
				this.StepBackward();
				return this.ParseEntity();
		}
	}

}
}

module.exports.PBXParser = PBXParser;
