var fs = require('fs');
var node_path = require('path');
var PBXParser = require("./PBXEditor/PBXParser").PBXParser;
var PBXSortedDictionary = require("./PBXEditor/PBXSortedDictionary").PBXSortedDictionary;
var PBXDictionary = require("./PBXEditor/PBXDictionary").PBXDictionary;
var PBXGroup = require("./PBXEditor/PBXGroup").PBXGroup;
var PBXList=require("./PBXEditor/PBXList").PBXList;
var PBXFileReference=require('./PBXEditor/PBXFileReference').PBXFileReference;
var PBXBuildFile=require('./PBXEditor/PBXBuildFile').PBXBuildFile;
var PBXProject=require('./PBXEditor/PBXProject').PBXProject;

function XCProject(pathToBuiltProject) {	

	//  xcode项目路径
	this.dataPath=node_path.join(pathToBuiltProject,"..");
	
	// xcode项目文件夹路径
	this.projectRootPath = "";

	// pathToBuiltProject项目配置文件路径
	this.filePath =pathToBuiltProject ;
	//"Users/demon/Documents/project/ddianle/0617/0617_Pay/Unity-iPhone.xcodeproj/";

	// 项目配置文件
	// var projectFileInfo;

	// PBXProject
	this._project = null;

	// PBXGroup
	this._rootGroup = null;

	// 项目配置文件字典集
	this._datastore=null;

	this._objects=null;

	this._rootObjectKey=null;

	this.modified = false;

	// Data Begin
	this._buildFiles = null;

	this._groups = null;

	this._fileReferences = null;

	this._nativeTargets = null;

	this._fileReferences = null;

	this._nativeTargets = null;

	this._frameworkBuildPhases = null;

	this._resourcesBuildPhases = null;

	this._shellScriptBuildPhases =null;

	this._sourcesBuildPhases = null;

	this._copyBuildPhases = null;

	this._buildConfigurations = null;

	this._configurationLists = null;

	// Data End

	//TODO 判断路径 EndsWith(".xcodeproj")

	var contents = fs.readFileSync("/"+this.filePath + "project.pbxproj") + "";

	var parser = new PBXParser();
	this._datastore = parser.Decode(contents);
	
	if (this._datastore._data == null) {
		console.log("Project file not found at file path " + this.filePath);
	}

	if (this._datastore._data["objects"] == "") {
		console.log("Errore " +this._datastore._data.length);
		return;
	}

	this._objects = this._datastore._data["objects"];
	
	this.modified = false;

	this._rootObjectKey = this._datastore._data["rootObject"];	
	
	if (this._rootObjectKey != null) {
		
		this._project = new PBXProject(this._rootObjectKey,this._objects._data[this._rootObjectKey]);
		
		this._rootGroup =new PBXGroup(this._rootObjectKey,this._objects._data[this._project.mainGroupID()]); 
	} else {
		console.log("error: project has no root object");
		this._project = null;
		this._rootGroup = null;
	}	
};


XCProject.prototype={
	
		GetFile : function(name) {
			
			if (name == "" || name==null) {
				return null;
			}

			this.fileReferences();
			
			for(var i=0;i<this._fileReferences._data.length;i++) {
				var fileReferencesObj=this._fileReferences._data[i];
				for(var k in fileReferencesObj) {
					var _name=fileReferencesObj[k].name();
					if (_name != "" && _name !=null && _name == name) {
						return fileReferencesObj[k];
					}
				}
			}

			return null;
		},

		GetGroup : function(name, path, parent) { 
			
			if(typeof(path)=="undefined") {
				path=null;
			}
			if(typeof(parent)=="undefined") {
				parent=null;
			}
			
			if (name == "" || name==null) {
				return null;
			}

			if (parent == null) {
				parent = this._rootGroup;
			}

			this.groups();
			
			for(var i=0;i<this._groups._data.length;i++) {
				var groupObj=this._groups._data[i];
				for(var k in groupObj) {
					var _name=groupObj[k].name();
					if(_name==null || _name=="") {
						var _path=groupObj[k].path();
						if(_path==name && parent.HasChild(k)) {
							return groupObj[k];
						}
					} else if(_name==name && parent.HasChild(k)) {
						return groupObj[k];
					}					
				}
			}

			var result = new PBXGroup(name,path);
			this.groups().Add(result);
			parent.AddChild(result);			
			modified=true;		
				
			return result;
		},
		
		AddFolderReference : function(folderName,parent) { 
			
			if(typeof(folderPath)=="undefined") {
				folderPath=null;
			}
			if(typeof(parent)=="undefined") {
				parent=null;
			}
			
			if(parent==null) {
				parent=this._rootGroup;
			}
			
			var fileReference = this.GetFile(folderName);
			
			if (fileReference != null) {
				console.log("File already exists: " + folderName);
				return null;
			}
			
			fileReference=new PBXFileReference(folderName,"GROUP");
			parent.AddChild(fileReference);
			this._fileReferences.Add(fileReference);
			// results.Add(fileReference.guid(),fileReference);
			
			this.resourcesBuildPhases();
			
			for(var k in this._resourcesBuildPhases._data) {							
				this.BuildAddFile(fileReference,this._resourcesBuildPhases._data[k]);
			}			
			
		},
		
		AddFolder : function(folderPath,parent,exclude,recursive,createBuildFile) {
			
			if(typeof(folderPath)=="undefined") {
				folderPath=null;
			}
			if(typeof(parent)=="undefined") {
				parent=null;
			}
			if(typeof(exclude)=="undefined") {
				exclude=null;
			}
			if(typeof(recursive)=="undefined") {
				recursive=true;
			}
			if(typeof(createBuildFiles)=="undefined") {
				createBuildFiles=true;
			}
			
			if(!fs.existsSync("/"+folderPath)) {
				return false;
			}
			if(exclude==null) {
				exclude=[];
			}
			if(parent==null) {
				parent=this._rootGroup;
			}
            
            var basename = node_path.basename("/"+folderPath);
            var f = null;
            if(fs.statSync("/"+folderPath).isDirectory()) {
                f = basename;
            }
            
			var newGroup=this.GetGroup(basename,f,parent);
			
			var r_arr=this.GetDirAndFile(folderPath,exclude);
			var dirs=r_arr[0];
			var files=r_arr[1];
			for(var i=0;i<dirs.length;i++) {
				var dir=dirs[i];
				var houzhui="";
				houzhui=dir.split(".").pop();
				// 正则取后缀名
				//if(/\.[^\.]+$/.exec(dir)[0]==".bundle") {
				if(houzhui=="bundle") {
					this.AddFile(dir,newGroup,"SOURCE_ROOT",createBuildFiles);
					continue;
				}
				if(houzhui=="framework") {
					this.AddFile(dir,newGroup,"SOURCE_ROOT",createBuildFiles);
					continue;
				}
				if(recursive) {
					this.AddFolder(dir,newGroup,exclude,recursive,createBuildFiles);
				}				
			}
			
			var regexExclude=exclude.join('|');
			var reg=new RegExp(regexExclude);
			for(var i=0;i<files.length;i++) {
				if(reg.test(files[i])) {
					continue;
				}
				this.AddFile(files[i],newGroup,"SOURCE_ROOT",createBuildFiles);
			}
			modified=true;
			return modified;
			
		},
		GetDirAndFile : function(dirPath,exclude) {
			var r_arr=[];
			var d_arr=[];
			var f_arr=[];
			var arr=fs.readdirSync("/"+dirPath);
			
			var regexExclude=exclude.join('|');
			var reg=new RegExp(regexExclude);
			
			for(var i=0;i<arr.length;i++) {
				var p=node_path.join(dirPath,arr[i]);
				
				if(reg.test(p)){
					continue;
				}			
				
				if(fs.statSync("/"+p).isDirectory()) {
					d_arr.push(p);
				} else {
					f_arr.push(p);
				} 
			}	
			r_arr[0]=d_arr;
			r_arr[1]=f_arr;			
			
			return r_arr;
		},

		AddFile : function(filePath, parent, tree, createBuildFiles, weak) { 
			
			if(typeof(filePath)=="undefined") {
				filePath=null;
			}
			if(typeof(parent)=="undefined") {
				parent=null
			}
			if(typeof(tree)=="undefined") {
				tree="SOURCE_ROOT";
			}
			if(typeof(createBuildFiles)=="undefined") {
				createBuildFiles=true;
			}
			if(typeof(weak)=="undefined") {
				weak=false;
			}
			
			var results = new PBXDictionary();
			if (filePath == null) {
				//console.log("AddFile called with null filePath");
				return results;
			}

			var absPath = "";
			if(fs.existsSync("/"+filePath)) {
				absPath=filePath;
			} else if(tree!="SDKROOT") {
				absPath=node_path.join(this.dataPath,filePath);
			}
			
			if(!fs.existsSync("/"+absPath) && tree!="SDKROOT") {
				console.log("Missing file: " + filePath);
				return results;
			} else if(tree =="SOURCE_ROOT") {
				filePath=absPath.replace(this.dataPath+"/","");// 改成相对路径
			}
			
			if (parent == null) {
				parent =this._rootGroup;
			}

			var fileReference = this.GetFile(node_path.basename(filePath));
			
			if (fileReference != null) {
				console.log("File already exists: " + filePath);
				return null;
			}
			
			fileReference=new PBXFileReference(filePath,tree);
			parent.AddChild(fileReference);
			this._fileReferences.Add(fileReference);
			results.Add(fileReference.guid(),fileReference);
			
			if(fileReference.buildPhase!="" && fileReference.buildPhase !=null && createBuildFiles) {
				switch(fileReference.buildPhase) {
					case "PBXFrameworksBuildPhase" :
						
						this.frameworkBuildPhases();
																									
						for(var k in this._frameworkBuildPhases._data) {
							this.BuildAddFile(fileReference,this._frameworkBuildPhases._data[k],weak);
						}
						
						if(absPath!=null && absPath !="" && tree=="SOURCE_ROOT") {
							var libraryPath=node_path.join('$(SRCROOT)',node_path.dirname(filePath));
							if(fs.existsSync("/"+absPath) && fs.statSync("/"+absPath).isFile()) {
								this.AddLibrarySearchPaths(new PBXList(libraryPath));
							} else {
								this.AddFrameworkSearchPaths(new PBXList(libraryPath));								
							}
						}
						
						break;
					case "PBXResourcesBuildPhase" :
						
						this.resourcesBuildPhases();
						
						for(var k in this._resourcesBuildPhases._data) {							
							this.BuildAddFile(fileReference,this._resourcesBuildPhases._data[k],weak);
						}
						break;
					case "PBXShellScriptBuildPhase" :
						
						this.shellScriptBuildPhases();
						
						for(var k in this._shellScriptBuildPhases._data) {							
							this.BuildAddFile(fileReference,this._shellScriptBuildPhases._data[k],weak);
						}
						break;
					case "PBXSourcesBuildPhase" :
						
						this.sourcesBuildPhases();
						
						for(var k in this._sourcesBuildPhases._data) {							
							this.BuildAddFile(fileReference,this._sourcesBuildPhases._data[k],weak);
						}
						break;
					case "PBXCopyFilesBuildPhase" :
						
						this.copyBuildPhases();
						
						for(var k in this._copyBuildPhases._data) {							
							this.BuildAddFile(fileReference,this._copyBuildPhases._data[k],weak);
						}
						break;
					case null :						
						//console.log("File Not Supported: " + filePath);
						break;
					default :
						//console.log("File Not Supported.");
						return null;
				}
			}
			return results;
		},
		
		BuildAddFile : function(fileReference,currentObject,weak) {			
			this.buildFiles();
			var buildFile= new PBXBuildFile(fileReference,weak);
			this._buildFiles.Add(buildFile);
			currentObject.AddBuildFile(buildFile);	
		},

		buildFiles : function() {
			if(this._buildFiles == null) {
				this._buildFiles = new PBXSortedDictionary(this._objects, "PBXBuildFile");
			}
			return this._buildFiles;
		},

		groups : function() {
			if(this._groups==null) {
				this._groups = new PBXSortedDictionary(this._objects, "PBXGroup");
			}
			return this._groups;
		},

		fileReferences : function() {
			if(this._fileReferences==null) {
				this._fileReferences = new PBXSortedDictionary(this._objects, "PBXFileReference");
			}
			return this._fileReferences;
		},

		nativeTargets : function() {
			if(this._nativeTargets==null) {
				this._nativeTargets = new PBXDictionary(this._objects, "PBXNativeTarget");
			}
			return this._nativeTargets;
		},

		buildConfigurations : function() {
			if(this._buildConfigurations==null) {
				this._buildConfigurations = new PBXDictionary(this._objects, "XCBuildConfiguration");
			}
			return this._buildConfigurations;
		},

		configurationLists : function() {
			if(this._configurationLists==null) {
				this._configurationLists = new PBXSortedDictionary(this._objects, "XCConfigurationList");
			}
			return this._configurationLists;
		},

		frameworkBuildPhases : function() {
			if(this._frameworkBuildPhases==null) {
				this._frameworkBuildPhases = new PBXDictionary(this._objects, "PBXFrameworksBuildPhase");
			}
			return this._frameworkBuildPhases;
		},

		resourcesBuildPhases : function() {
			if(this._resourcesBuildPhases==null) {
				this._resourcesBuildPhases = new PBXDictionary(this._objects, "PBXResourcesBuildPhase");
			}
			return this._resourcesBuildPhases;
		},

		shellScriptBuildPhases : function() {
			if(this._shellScriptBuildPhases==null) {
				this._shellScriptBuildPhases = new PBXDictionary(this._objects, "PBXShellScriptBuildPhase");
			}
			return this._shellScriptBuildPhases;
		},

		sourcesBuildPhases : function() {
			if(this._sourcesBuildPhases==null) {
				this._sourcesBuildPhases = new PBXDictionary(this._objects, "PBXSourcesBuildPhase");
			}
			return this._sourcesBuildPhases;
		},

		copyBuildPhases : function() {
			if(this._copyBuildPhases==null) {
				this._copyBuildPhases = new PBXDictionary(this._objects, "PBXCopyFilesBuildPhase");
			}
			return this._copyBuildPhases;
		},

		AddOtherCFlags : function(arg1) {
			var flags=arg1;
			if(typeof(flags)=="string") {
				flags=new PBXList(flags);
			}
			
			this.buildConfigurations();		
			for(var k in this._buildConfigurations._data) {
				this._buildConfigurations._data[k].AddOtherCFlags(flags);
			}
			
			modified=true;
			return modified;				
		},
		AddOtherLinkerFlags : function(arg1) {
			var flags=arg1;
			if(typeof(flags)=="string") {
				flags=new PBXList(flags);
			}
			
			this.buildConfigurations();			
			for(var k in this._buildConfigurations._data) {
					this._buildConfigurations._data[k].AddOtherLinkerFlags(flags);
			}
			
			modified=true;
			return modified;
		},
		AddHeaderSearchPaths : function(arg1) {
			var paths=arg1;
			
			if(typeof(paths)=="string") {
				paths=new PBXList(paths);
			}
			
			this.buildConfigurations();			
			
			for(var k in this._buildConfigurations._data) {
				this._buildConfigurations._data[k].AddHeaderSearchPaths(paths);
			}
			
			modified=true;
			return modified;
		},
		AddLibrarySearchPaths : function(arg1) {
			var paths=arg1;
			
			if(typeof(paths)=="string") {
				paths=new PBXList(paths);
			}
			
			this.buildConfigurations();
			
			for(var k in this._buildConfigurations._data ) {			
					this._buildConfigurations._data[k].AddLibrarySearchPaths(paths);								
			}
			
			modified=true;
			return modified;
		},
		AddFrameworkSearchPaths : function(arg1) {
			var paths=arg1;
			
			if(typeof(paths)=="string") {
				paths=new PBXList(paths);
			}
			
			this.buildConfigurations();
			
			for(var k in this._buildConfigurations._data ) {
					this._buildConfigurations._data[k].AddFrameworkSearchPaths(paths);				
			}
			modified=true;
			return modified;	
		},
		GetObject : function(guid) {
			return this._objects._data[guid];
		},
		ApplyMod : function(mod) {
						
			var modGroup = this.GetGroup(mod["group"]);

			//console.log("Adding libraries...");	
			for (var i = 0; i < mod["libs"].length; i++) {
				var libRef = mod["libs"][i];
				var completeLibPath = "usr/lib/" + libRef;
				this.AddFile(completeLibPath, modGroup, "SDKROOT", true, libRef["isWeak"]);
			}


			//console.log("Adding frameworks...");
			var frameworkGroup = this.GetGroup("Frameworks");
			for (var i=0;i< mod["frameworks"].length;i++) {
				var framework=mod["frameworks"][i];
				var filename = framework.split(":");
				var isWeak = (filename.length > 1) ? true : false;
				
				var completePath =node_path.join("System/Library/Frameworks",filename[0]);
				this.AddFile(completePath, frameworkGroup, "SDKROOT", true, isWeak);
			}

			//console.log("Adding files...");
			for (var i=0;i<mod["files"].length;i++) {
				var filePath=mod["files"][i];
				var absoluteFilePath =node_path.join(this.dataPath,filePath);
				this.AddFile(absoluteFilePath,modGroup);
			}

			//console.log("Adding folders...");
			for (var i=0;i<mod["folders"].length;i++) {
				var folderPath=mod["folders"][i];
				var filename = folderPath.split(":");
				var isRef = (filename.length > 1) ? true : false;
				if(isRef) {
					this.AddFolderReference(filename[0],this._rootGroup);
				} else {
					var absoluteFolderPath = node_path.join(this.dataPath,folderPath);
					this.AddFolder(absoluteFolderPath,modGroup,mod["excludes"]);
				}
			}

			//console.log("Adding headerpaths...");
			for (var i=0;i<mod["headerpaths"].length;i++) {
				var headerpath = mod["headerpaths"][i];
				if (/\$\(inherited\)/.test(headerpath)) {
					console.log("not prepending a path to " + headerpath);
					this.AddHeaderSearchPaths(headerpath);
				} else {
					var absoluteHeaderPath = node_path.join("/"+this.dataPath,headerpath);
					this.AddHeaderSearchPaths(absoluteHeaderPath);
				}
			}

			//console.log("Adding compiler flags...");
			for (var i=0;i<mod["compiler_flags"].length;i++) {
				var flag=mod["compiler_flags"][i];				
				this.AddOtherCFlags(flag);
			}

			//console.log("Adding linker flags...");
			for (var i=0;i<mod["linker_flags"].length;i++) {
				var flag=mod["linker_flags"][i];
				this.AddOtherLinkerFlags(flag);
			}

			this.Consolidate();
		},

		// Savings
		Consolidate : function() {
			var consolidated = new PBXDictionary();
			consolidated.Append(this.buildFiles());
			//PBXSortedDictionary
			consolidated.Append(this.copyBuildPhases());
			//PBXDictionary
			consolidated.Append(this.fileReferences());
			//PBXSortedDictionary
			consolidated.Append(this.frameworkBuildPhases());
			//PBXDictionary
			consolidated.Append(this.groups());
			//PBXSortedDictionary
			consolidated.Append(this.nativeTargets());
			//PBXDictionary
			consolidated.Add(this._project.guid(),this._project); 
			consolidated.Append(this.resourcesBuildPhases());
			//PBXDictionary
			consolidated.Append(this.shellScriptBuildPhases());
			//PBXDictionary
			consolidated.Append(this.sourcesBuildPhases());
			//PBXDictionary
			consolidated.Append(this.buildConfigurations());
			//PBXDictionary
			consolidated.Append(this.configurationLists());
			//PBXSortedDictionary
			this._objects = consolidated;
			consolidated = null;
		},
		overwriteBuildSetting : function(settingName,newValue,buildConfigName,isstring) {
			
			if(typeof(buildConfigName)=="undefined") {
				buildConfigName="all";
			}
			
			this.buildConfigurations();

			for(var k in this._buildConfigurations._data) {
				var b=this._buildConfigurations._data[k];
				if(b.data()["name"]==buildConfigName || b.data()["name"]=="all") {
					this._buildConfigurations._data[k].overwriteBuildSetting(settingName,newValue,isstring);
					modified=true;
				} else {

				}
			}
			return modified;
		},	
		CreateNewProject : function(result,path,callback) {			
			var parser = new PBXParser();
			//创建文件
			var res=parser.Encode(result,true);
			//保存文件	
			fs.writeFile(path,res,function(err){
				// console.log("OK");
				if(err) {
					throw err;
				}				
				callback();				
			});
		},
		
		Save : function(callback) {
			var result=new PBXDictionary();
			result.Add("archiveVersion",1);
			result.Add("classes",new PBXDictionary());
			result.Add("objectVersion",46);
			
			this.Consolidate();
			
			result.Add("objects",this._objects);
			result.Add("rootObject",this._rootObjectKey);
			
			var projectPath=node_path.join("/"+this.filePath,"project.pbxproj");
			// Delete old project
			
			// Pares result object directly into file
			this.CreateNewProject(result,projectPath,callback);
		}
}

module.exports.XCProject = XCProject;
