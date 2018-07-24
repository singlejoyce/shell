#!/bin/sh

########################################################################### 
echo "prepare workspace"
cur_path=`dirname $0`
cd $cur_path
cur_path=`pwd`
echo "cd to work path: $cur_path"

log_dir=log

greplog="$log_dir/greplog.txt"
templog="$log_dir/templog.txt"
findresult="$log_dir/findresult.txt"
newresult="$log_dir/newresult.txt"
adb_assetbundle_successfile="$log_dir/adb_assetbundle_successfile.log"
adb_assetbundle_failfile="$log_dir/adb_assetbundle_failfile.log"
ios_assetbundle_successfile="$log_dir/ios_assetbundle_successfile.log"
ios_assetbundle_failfile="$log_dir/ios_assetbundle_failfile.log"
adb_stage_successfile="$log_dir/adb_stage_successfile.log"
adb_stage_failfile="$log_dir/adb_stage_failfile.log"
ios_stage_successfile="$log_dir/ios_stage_successfile.log"
ios_stage_failfile="$log_dir/ios_stage_failfile.log"
controller_successfile="$log_dir/controller_successfile.log"
controller_failfile="$log_dir/controller_failfile.log"
songsmp_successfile="$log_dir/songsmp_successfile.log"
songsmp_failfile="$log_dir/songsmp_failfile.log"

songfile="song.txt"
song_stage="$cur_path/log-song-stage.txt"
song_controller="$cur_path/log-song-controller.txt"
song_smp="$cur_path/log-song-smp.txt"


rm -rf $log_dir $song_stage $song_controller $song_smp
mkdir -p $log_dir || (echo "log_dir Directory can NOT be created ." && exit 1)

########################################################################### 
#adb_server_assetbundleDir="svn://172.16.1.237/branch/Resource_N1/Res/Animations"
#ios_server_assetbundleDir="svn://172.16.1.237/branch/Resource_N1_Unity4.7.2/Res/Animations"
#server_stage="svn://172.16.1.237/branch/Resource_N1/Data/stage"
#server_musicsmp="svn://172.16.1.237/branch/Resource_N1/Res/Music"

adb_assetbundleDir="$cur_path/adb_Animations"
ios_assetbundleDir="$cur_path/ios_Animations"
stage_dir="$cur_path/stage"
musicsmp_dir="$cur_path/Music"
controller_dir="$adb_assetbundleDir/Controller"

#路径执行svn更新操作
echo "start to svn update all dir:"
svn update $adb_assetbundleDir --username build --password dailybuild
svn update $ios_assetbundleDir --username build --password dailybuild
svn update $stage_dir --username build --password dailybuild
svn update $musicsmp_dir --username build --password dailybuild
echo "svn update all dir end!"

########################################################################### 
sed -i '1d' $songfile
cat $cur_path/$songfile|awk -F"[\t]" '{

/*获取歌曲id，格式为3位数的补0，4位数的不操作，然后统一转换格式为song歌曲id.*/
if(length($1)==3) {$1="song0"$1"."; }
else if(length($1)==4) { $1="song"$1".";}

/*获取模式id，转换为缩写格式，并标注所在文件夹目录*/
if($3==1) {$3="tg"; $5="taigu" }
else if($3==2){ $3="td";  $5="tradition"} 
else if($3==3){ $3="os";  $5="osu"} 
else if($3==4){ $3="au";  $5="au"} 
else if($3==5){ $3="rh";  $5="rhythm"} 
else if($3==11){ $3="sg";  $5="taigu"} 
else if($3==12){ $3="sd";  $5="tradition"} 
else if($3==13){ $3="so";  $5="osu"} 
else if($3==14){ $3="su";  $5="au"} 
else if($3==21){ $3="hb";  $5="heartbeats"} 

/*获取难度level，转换为缩写格式*/
if($4==1) {$4="e";  }
else if($4==2){ $4="n";  } 
else if($4==3){ $4="h";  } 

/*将以上三个数据拼接后输出到文件*/
if($1>0) {print $5"/"$1 $3 $4 > "log-song-stage.txt";  }
}'

########################################################################### 
sed -i '1d' $songfile
cat $cur_path/$songfile|awk -F"[\t]" '{

/*获取歌曲id，格式为3位数的补0，4位数的不操作，然后统一转换格式为song歌曲id.*/
if(length($1)==3) {$1="song0"$1; }
else if(length($1)==4) { $1="song"$1;}

/*获取模式id，转换为缩写格式，并标注所在文件夹目录*/
if($3==1) {$3=""; $5="Dance.txt" }
else if($3==2){ $3="";  $5="Dance.txt"} 
else if($3==3){ $3="";  $5="Dance.txt"} 
else if($3==4){ $3="";  $5="Dance.txt"} 
else if($3==5){ $3="";  $5="Dance.txt"} 
else if($3==11){ $3="";  $5="Dance.txt"} 
else if($3==12){ $3="";  $5="Dance.txt"} 
else if($3==13){ $3="";  $5="Dance.txt"} 
else if($3==14){ $3="";  $5="Dance.txt"} 
else if($3==21) {$3="_double";  $5="Dance.txt"}

/*将以上三个数据拼接后输出到文件*/
if($1>0) {print $1$3"/"$5 > "log-song-controller.txt";  }
}'

########################################################################### 
sed -i '1d' $songfile
cat $cur_path/$songfile|awk -F"[\t]" '{

/*获取歌曲id，格式为3位数的补0，4位数的不操作，然后统一转换格式为song歌曲id.*/
if(length($1)==3) {$1="song0"$1".smp"; }
else if(length($1)==4) { $1="song"$1".smp";}

/*输出到文件*/
if($1>0) {print $1 > "log-song-smp.txt";  }
}'

########################################################################### 
#检查android目录下的关卡文件
echo "start to check adb stage:"
cd $stage_dir/android
cat $song_stage| while read keyword;
do
	if [ -f "$keyword" ]; then
		echo "$keyword:success"  >> $cur_path/$adb_stage_successfile
	else
		echo "$keyword" >> $cur_path/$adb_stage_failfile
		echo "android stage find failed!!!!!failed file name is: $keyword" 
	fi
done

#检查ios目录下的关卡文件
echo "start to check ios stage:"
cd $stage_dir/ios
cat $song_stage| while read keyword;
do
	if [ -f "$keyword" ]; then
		echo "$keyword:success"  >> $cur_path/$ios_stage_successfile
	else
		echo "$keyword" >> $cur_path/$ios_stage_failfile
		echo "ios stage find failed!!!!!failed file name is: $keyword" 
	fi
done

#检查Animations\Controller目录下的Dance.txt文件
echo "start to check Controller Dance.txt:"
cd $controller_dir/
cat $song_controller| while read keyword;
do
	if [ -f "$keyword" ]; then
		echo "$keyword:success"  >> $cur_path/$controller_successfile
	else
		echo "$keyword" >> $cur_path/$controller_failfile
		echo "Controller find failed!!!!!failed file name is: $keyword"
	fi
done

#检查Music目录下的songxxxx.smp文件
echo "start to check music songxxxx.smp:"
cd $musicsmp_dir/
cat $song_smp| while read keyword;
do
	if [ -f "$keyword" ]; then
		echo "$keyword:success"  >> $cur_path/$songsmp_successfile
	else
		echo "$keyword" >> $cur_path/$songsmp_failfile
		echo "Controller find failed!!!!!failed file name is: $keyword"
	fi
done

########################################################################### 
#查找所有包含动作的行并输出到文件中
cd $adb_assetbundleDir
find $controllerDir -name "Dance.txt"| xargs grep "Motion:" | while read line;
do
	echo "$line" >> $cur_path/$greplog
	filename=${line##*Motion:}   
	echo "$filename" >> $cur_path/$templog
done

#修正文件格式
sed -i 's/\r//' $cur_path/$templog

#内容全部转成大写
tr 'a-z' 'A-Z'  < $cur_path/$templog > $cur_path/$findresult

#每行后增加固定后缀名并输出到文件中
sed 's/$/&.assetbundle/g' $cur_path/$findresult > $cur_path/$newresult

#开始检查安卓动作文件
echo "start to check adb assetbundle:"
cat $cur_path/$newresult| while read keyword;
do
	if [ -f "$keyword" ]; then
		echo "$keyword:success" >> $cur_path/$adb_assetbundle_successfile
	else
		echo "$keyword" >> $cur_path/$adb_assetbundle_failfile
		echo "find adb assetbundle failed!!!!!failed file name is: $keyword" 
	fi
done

#开始检查ios动作文件
echo "start to check ios assetbundle:"
cd $ios_assetbundleDir
cat $cur_path/$newresult| while read keyword;
do
	if [ -f "$keyword" ]; then
		echo "$keyword:success" >> $cur_path/$ios_assetbundle_successfile
	else
		echo "$keyword" >> $cur_path/$ios_assetbundle_failfile
		echo "find ios assetbundle failed!!!!!failed file name is: $keyword" 
	fi
done
