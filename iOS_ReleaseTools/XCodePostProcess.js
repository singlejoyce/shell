var XCProject = require("./XCProject").XCProject;

// var mod = {
// 	"group": "GameCenter",
// 	"libs": ["libpq.5.5.dylib"],
// 	"frameworks": ["StoreKit.framework","AppKit.framework"],
// 	"headerpaths": ["iOS/GameCenter/**"],
// 	"files": ["iOS/GameCenter/GKWrapper.m",
// 		"iOS/GameCenter/GKWrapper.h",
// 		"iOS/Store/libStoreKit.a"
// 	],
// 	"folders": ["Res:ref"],
// 	"excludes": ["^.*.meta$", "^.*.mdown$", "^.*.pdf$"],
// 	"linker_flags": ["-ObjC"],
// 	"compiler_flags":["-ObjC"]
// }

//文件夹引用  buildfile

function XCodePostProcess() {

	this.OnPostProcessBuild = function(platformconfig,pathToBuiltProject,callback) {

		var project = new XCProject(pathToBuiltProject);
		for(var i=0;i<platformconfig.mods.length;i++) {
			project.ApplyMod(platformconfig.mods[i]);
		}		

		project.overwriteBuildSetting("CODE_SIGN_IDENTITY[sdk=iphoneos*]", platformconfig.dissign, "Release",true);
		project.overwriteBuildSetting("CODE_SIGN_IDENTITY[sdk=iphoneos*]", platformconfig.dissign, "Debug",true);
		
		project.overwriteBuildSetting("ENABLE_BITCODE", 'NO', "Release",true);
		project.overwriteBuildSetting("ENABLE_BITCODE", 'NO', "Debug",true);

		//string yes
		project.overwriteBuildSetting("PROVISIONING_PROFILE",platformconfig.pubprofile,"Release",true);
		project.overwriteBuildSetting("PROVISIONING_PROFILE",platformconfig.pubprofile,"Debug",true);

		project.Save(callback);

	}
}


// var xcodePostProcess = new XCodePostProcess();
// xcodePostProcess.OnPostProcessBuild([mod],"",function(msg){});
