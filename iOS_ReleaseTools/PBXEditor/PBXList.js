function PBXList(firstValue) {
	
	this.typeName="PBXList";	
	
	this._data=[];	
	if(typeof(firstValue)!="undefined") {
		this._data.push(firstValue);
	}	
}

PBXList.prototype={
	ToCSV : function() {
		var ret = "";
		for(var i=0;i<this._data.length;i++) {
			ret += '\"';
			ret += this._data[i];
			ret += '\",';
		}
		return ret;
	},
	ToString : function () {
		return "{"+this.ToCSV+"}";
	},
	Add : function (v) {
		this._data.push(v);
	},
	Contains : function (arg1) {
	
		for(var i=0;i<this._data.length;i++) {
			if(this._data[i]==arg1) {
				return true;
			}
		}
	
		return false;
	}
};


module.exports.PBXList=PBXList;