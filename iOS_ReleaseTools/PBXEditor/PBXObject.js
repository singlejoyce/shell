var PBXDictionary = require("./PBXDictionary").PBXDictionary;

	
	var ISA_KEY = "isa";
	
	function PBXObject() {

		this._guid = null;
		this._data = null;

		this.typeName = "PBXObject";
	}

	PBXObject.prototype = {
		guid: function() {
			if (this._guid == null) {
				this._guid = Guid.NewGuid().ToString("N").toUpperCase().substring(8);
			}
			return this._guid;
		},

		data: function() {
			if (this._data == null) {
				this._data = new PBXDictionary();
			}
			return this._data._data;
		},

		Init: function() {
			this._data = new PBXDictionary();
			this._data._data[ISA_KEY] = this.typeName;
			this._guid = Guid.NewGuid().ToString("N").toUpperCase().substring(8);
		},

		InitWithGuid: function(guid) {
			this.Init();
			this._guid = guid;
		},

		InitWithGuidAndDic: function(guid, dic) {

			this.InitWithGuid(guid);
			
			if (typeof(dic._data[ISA_KEY]) == "undefined" || dic._data[ISA_KEY] != this.typeName) {
				//console.log("PBXDictionary is not a valid ISA object");
			}

			for (var k in dic._data) {
				this._data._data[k] = dic._data[k];
			}
		},

		Add: function(key, obj) {
			this._data._data[key]=obj;
		},

		Remove: function(key) {
			return delete this._data._data[key];
		},

		ContainsKey: function(key) {
			return typeof(this._data._data[key]) != "undefined";
		},
		IsGuid: function(aString) {
			return aString.match(/^[A-Fa-f0-9]{24}$/);
		},
		ToString: function() {
			var ret = "";
			for (var key in this._data._data) {
				ret += "<";
				ret += key;
				ret += ", ";
				ret += this._data._data[key];
				ret += ">, ";
			}
			return '{"' + ret + '"},';
		}
	};

	//表示全局唯一标识符 (GUID)。

	function Guid(g) {

		var arr = new Array(); //存放32位数值的数组



		if (typeof(g) == "string") { //如果构造函数的参数为字符串

			InitByString(arr, g);

		} else {

			InitByOther(arr);

		}

		//返回一个值，该值指示 Guid 的两个实例是否表示同一个值。

		this.Equals = function(o) {

			if (o && o.IsGuid) {

				return this.ToString() == o.ToString();

			} else {

				return false;

			}

		}

		//Guid对象的标记

		this.IsGuid = function() {}

		//返回 Guid 类的此实例值的 String 表示形式。

		this.ToString = function(format) {

			if (typeof(format) == "string") {

				if (format == "N" || format == "D" || format == "B" || format == "P") {

					return ToStringWithFormat(arr, format);

				} else {

					return ToStringWithFormat(arr, "D");

				}

			} else {

				return ToStringWithFormat(arr, "D");

			}

		}

		//由字符串加载

		function InitByString(arr, g) {

			g = g.replace(/\{|\(|\)|\}|-/g, "");

			g = g.toLowerCase();

			if (g.length != 32 || g.search(/[^0-9,a-f]/i) != -1) {

				InitByOther(arr);

			} else {

				for (var i = 0; i < g.length; i++) {

					arr.push(g[i]);

				}

			}

		}

		//由其他类型加载

		function InitByOther(arr) {

			var i = 32;

			while (i--) {

				arr.push("0");

			}

		}

		/*

	     根据所提供的格式说明符，返回此 Guid 实例值的 String 表示形式。

	     N  32 位： xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

	     D  由连字符分隔的 32 位数字 xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx

	     B  括在大括号中、由连字符分隔的 32 位数字：{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}

	     P  括在圆括号中、由连字符分隔的 32 位数字：(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)

	     */

		function ToStringWithFormat(arr, format) {

			switch (format) {

				case "N":

					return arr.toString().replace(/,/g, "");

				case "D":

					var str = arr.slice(0, 8) + "-" + arr.slice(8, 12) + "-" + arr.slice(12, 16) + "-" + arr.slice(16, 20) + "-" + arr.slice(20, 32);

					str = str.replace(/,/g, "");

					return str;

				case "B":

					var str = ToStringWithFormat(arr, "D");

					str = "{" + str + "}";

					return str;

				case "P":

					var str = ToStringWithFormat(arr, "D");

					str = "(" + str + ")";

					return str;

				default:

					return new Guid();

			}

		}

	}

	//Guid 类的默认实例，其值保证均为零。

	Guid.Empty = new Guid();

	//初始化 Guid 类的一个新实例。

	Guid.NewGuid = function() {

		var g = "";

		var i = 32;

		while (i--) {

			g += Math.floor(Math.random() * 16.0).toString(16);

		}

		return new Guid(g);

	}

	module.exports.PBXObject = PBXObject;

	
	function PBXNativeTarget(guid,dictionary) {
		this.typeName="PBXNativeTarget";
		this.baseName="PBXObject";
		if(typeof(guid)=="undefined") {
			this.Init();
		} else {
			this.InitWithGuidAndDic(guid,dictionary);
		}
	}
	
	PBXNativeTarget.prototype=PBXObject.prototype;
	
	module.exports.PBXNativeTarget = PBXNativeTarget;
	
	function PBXContainerItemProxy(guid,dictionary) {
		this.typeName="PBXContainerItemProxy";
		this.baseName="PBXObject";
		if(typeof(guid)=="undefined") {
			this.Init();
		} else {
			this.InitWithGuidAndDic(guid,dictionary);
		}
	}
	
	PBXContainerItemProxy.prototype=PBXObject.prototype;
	
	module.exports.PBXContainerItemProxy = PBXContainerItemProxy;
	
	
	function PBXReferenceProxy(guid,dictionary) {
		this.typeName="PBXReferenceProxy";
		this.baseName="PBXObject";
		if(typeof(guid)=="undefined") {
			this.Init();
		} else {
			this.InitWithGuidAndDic(guid,dictionary);
		}
	}
	
	PBXReferenceProxy.prototype=PBXObject.prototype;
	
	module.exports.PBXReferenceProxy = PBXReferenceProxy;
	
	function PBXVariantGroup(guid,dictionary) {
		this.typeName="PBXVariantGroup";
		this.baseName="PBXObject";
		
		if(typeof(guid)=="undefined") {
			this.Init();
		} else {
			this.InitWithGuidAndDic(guid,dictionary);
		}
	}
	
	PBXVariantGroup.prototype=PBXObject.prototype;
	
	module.exports.PBXVariantGroup = PBXVariantGroup;
