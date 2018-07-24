devsign="iPhone Developer: shijun wang (3LHUC57LGR)"
dissign="iPhone Distribution: Shanghai Lelian Information Technology Co., Ltd (C259Q68AGC)"
inhoursesign="iPhone Developer: shijun wang (3LHUC57LGR)"
devprofile="0db2f7da-431e-48bc-99b4-68f09bf9ddc3"
pubprofile="60a78d87-6513-4f43-a89a-dbcacea0835d"
inhourseprofile="e72be56f-4d0e-493c-a2c1-afc70f0e7d81"
        
function updateXcodeSign() {
    
    var xcodeprojectpath="C:\Users\build\Desktop\project.pbxproj";
    
    var content=fs.readFileSync(xcodeprojectpath)+"";
    
    //release
    if(ipaType==0) {
        content=content.replace(/(iPhone Distribution: diandianle Information Technology Co\.\, Ltd \(X4NP9H459A\))/g,"iPhone Developer: shijun wang (3LHUC57LGR)");
        eval("var reg=/("+pubprofile+")/g");
        content=content.replace(reg,devprofile);
    } else if(ipaType==1) {//debug
        content=content.replace(/(iPhone Developer: shijun wang \(3LHUC57LGR\))/g,"iPhone Distribution: LoveDance Technology Co., Ltd.");
        eval("var reg=/("+devprofile+")/g");
        content=content.replace(reg,inhourseprofile);
    } else if(ipaType==2) {//inhourse
        content=content.replace(/(iPhone Distribution: LoveDance Technology Co\.\, Ltd\.)/g,"iPhone Distribution: diandianle Information Technology Co., Ltd (X4NP9H459A)");
        eval("var reg=/("+devprofile+")/g");
        content=content.replace(reg,pubprofile);
    }	

    fs.writeFileSync(xcodeprojectpath,content);
}

ipaType=0
while (ipaType>2)
    updateXcodeSign()
    ipaType=ipaType+1