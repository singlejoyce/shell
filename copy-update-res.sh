#!/bin/sh

#接收输入值
echo -n "请输入原目录的文件夹名称:"   #-n用于允许用户在字符串后面立即输入数据，而不是在下一行输入。
read name

#接收输入值
echo -n "请输入要操作的平台:(安卓-adb: 1, 越狱-iosroot: 2, ios正版-applefree: 3, 所有平台: 4)"   
read platform


#变量定义
adb_dir="/ddianle/$name/adb/adbres"
iosroot_dir="/ddianle/$name/adb/iosroot"
ios_dir="/ddianle/$name/ios"

adb_dir_test="/ddianle/test5/data/adbres"
iosroot_dir_test="/ddianle/test5/data/iosroot"
ios_dir_test="/ddianle/test5/data/applefree"


if [ $platform = 1 ]; then
	#拷贝安卓的资源
	echo "start to delete res!"
	rm -rf $adb_dir_test/res_all/
	echo "delete res success! & start to copy res! "
	\cp -fr $adb_dir/*.zip $adb_dir_test
	echo "adb res copy success! & start to unzip res! "
	unzip $adb_dir_test/res_all.zip -d $adb_dir_test
	echo "unzip res_all.zip success! & start to md5sum res! "
	echo "`md5sum $adb_dir_test/*.zip`"
elif [ $platform = 2 ] ; then
	#拷贝越狱的资源
	echo "start to delete res!"
	rm -rf $iosroot_dir_test/Res_All/
	echo "delete res success! & start to copy res!"
	\cp -fr $iosroot_dir/*.zip  $iosroot_dir_test
	echo "adb res copy success! & start to unzip res! "
	unzip $iosroot_dir_test/Res_All.zip -d $iosroot_dir_test
	echo "unzip res_all.zip success! & start to md5sum res!"
	echo "`md5sum $iosroot_dir_test/*.zip`"
elif [ $platform = 3 ] ; then
	#拷贝苹果正版的资源
	echo "start to delete res!"
	rm -rf $ios_dir_test/Res_All/
	echo "delete res success! & start to copy res!"
	\cp -fr $ios_dir/*.zip $ios_dir_test
	echo "adb res copy success! & start to unzip res! "
	unzip $ios_dir_test/Res_All.zip  -d $ios_dir_test
	echo "unzip res_all.zip success! & start to md5sum res!"
	echo "`md5sum $ios_dir_test/*.zip`"
elif [ $platform = 4 ] ; then
	#拷贝安卓的资源
	echo "start to delete res!"
	rm -rf $adb_dir_test/res_all/
	echo "delete res success! & start to copy res! "
	\cp -fr $adb_dir/*.zip $adb_dir_test
	echo "adb res copy success! & start to unzip res! "
	unzip $adb_dir_test/res_all.zip -d $adb_dir_test

	#拷贝越狱的资源
	echo "start to delete res!"
	rm -rf $iosroot_dir_test/Res_All/
	echo "delete res success! & start to copy res!"
	\cp -fr $iosroot_dir/*.zip  $iosroot_dir_test
	echo "adb res copy success! & start to unzip res! "
	unzip $iosroot_dir_test/Res_All.zip -d $iosroot_dir_test
	
	#拷贝苹果正版的资源
	echo "start to delete res!"
	rm -rf $ios_dir_test/Res_All/
	echo "delete res success! & start to copy res!"
	\cp -fr $ios_dir/*.zip $ios_dir_test
	echo "adb res copy success! & start to unzip res! "
	unzip $ios_dir_test/Res_All.zip  -d $ios_dir_test

	#md5sum zip
	#echo "`md5sum $adb_dir_test/*.zip`"
	#echo "`md5sum $ios_dir_test/*.zip`"
	#echo "`md5sum $ios_dir_test/*.zip`"
fi



