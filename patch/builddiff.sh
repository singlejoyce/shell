#!/bin/sh

########################################################################### cd to current path
cur_path=`dirname $0`
cd $cur_path
cur_path=`pwd`
echo "cd to work path: $cur_path"

########################################################################### get old version path
echo "get old version path"
old_ver_parent_dir=../backup/
old_ver_dir="";
for old_ver_dir_name in ${old_ver_parent_dir}/*; do
        if [[ "$old_ver_dir" = "" ]]; then
                old_ver_dir=$old_ver_dir_name
        fi
        if [[ "$old_ver_dir" < "$old_ver_dir_name" ]]; then
                old_ver_dir=$old_ver_dir_name
        fi
done
echo "old ver dir : $old_ver_dir"

########################################################################### get new version path
echo "get new version path"
new_ver_parent_dir=../
new_ver_dir="";
for new_ver_dir_name in ${new_ver_parent_dir}/*; do
        if [[ "$new_ver_dir" = "" ]]; then
                new_ver_dir=$new_ver_dir_name
        fi
        if [[ "$new_ver_dir" > "$new_ver_dir_name" ]]; then
                new_ver_dir=$new_ver_dir_name
        fi
done
echo "new ver dir : $new_ver_dir"

########################################################################### get next version
echo "get next version"
ver_zip_filename=`find $old_ver_dir -maxdepth 1 -type f |grep ".zip"|grep -v "base"|grep -v "all"|grep -v "res_all"`
cur_ver=`basename $ver_zip_filename .zip`   
echo "current version is $cur_ver"
next_ver=`expr $cur_ver + 1`
echo "next version is $next_ver"


########################################################################### unzip res
echo "unzip res to /xuanqu/lwts"
rm $old_ver_dir/xuanqu/ -rf
rm $new_ver_dir/xuanqu/ -rf
mkdir -p $old_ver_dir/xuanqu/lwts || (echo " Patch Directory can NOT be created ." && exit 1)
mkdir -p $new_ver_dir/xuanqu/lwts || (echo "Patch log Directory can NOT be created ." && exit 1)
unzip $old_ver_dir/all.zip -d $old_ver_dir/xuanqu/lwts
unzip $new_ver_dir/all.zip -d $new_ver_dir/xuanqu/lwts

########################################################################### Define param
old_dir=$old_ver_dir/xuanqu/lwts
new_dir=$new_ver_dir/xuanqu/lwts

patch_dir=patch
patch_log_dir=log

ignore_list=("./config/version.txt")

same_file="$patch_log_dir/samefile.txt"
update_file="$patch_log_dir/updatefile.txt"
add_file="$patch_log_dir/addfile.txt"
ignore_file="$patch_log_dir/ignorefile.txt"
########################################################################### Clean workspace
rm $patch_dir -rf
rm $patch_log_dir -rf
echo "clear workspace"

########################################################################### Prepare workspace
echo "prepare workspace"
mkdir -p $patch_dir || (echo " Patch Directory can NOT be created ." && exit 1)
mkdir -p $patch_log_dir || (echo "Patch log Directory can NOT be created ." && exit 1)

########################################################################### Make diff
echo "Make diff..."
cd ./$new_dir;
find . -type f | while read line;
do
	if [[ "${ignore_list[@]}" =~ $line ]]; then
		echo "ignore file $line" && continue
	fi
	if [ -f "$cur_path/$old_dir/$line" ]; then
		if [ "$(md5sum < $line)" = "$(md5sum < $cur_path/$old_dir/$line)" ]; then
			echo "$line" >> "$cur_path/$same_file"
		else

			if [ `echo "$line" | grep './res/Materials/' | grep '.clh'` ]; then
				echo "$line" >> "$cur_path/$ignore_file"
				echo "1.ignore Materials file： $line"
			elif [ `echo "$line" | grep './res/ClientVer/' | grep 'ResVer_Materials'` ]; then
				echo "$line" >> "$cur_path/$ignore_file"
				echo "ignore json file： $line"
			else
				echo "$line" >> "$cur_path/$update_file"
				size=$(stat -c %s $line)
				if [ $size -ge 1024000 ]; then
					echo "1.big file：$line Size=$size"
				fi
				mkdir -p $cur_path/$patch_dir/$(dirname $line) && cp $line $cur_path/$patch_dir/$line && continue
			fi
		fi
	else
		if [ `echo "$line" | grep './res/Materials/' | grep '.clh'` ]; then
			echo "$line" >> "$cur_path/$ignore_file"
			echo "2.ignore Materials file： $line"
		else
			echo "$line" >> "$cur_path/$add_file"
			size=$(stat -c %s $line)
			if [ $size -ge 1024000 ]; then
				echo "2.big file：$line Size=$size"
			fi
			mkdir -p $cur_path/$patch_dir/$(dirname $line) && cp $line $cur_path/$patch_dir/$line && continue
		fi
	fi
done

########################################################################### Make patch zip file
echo "make patch zip file : $next_ver.zip"
cd $cur_path/$patch_dir
zip -r -9 $next_ver.zip *
echo "make patch zip file complete"

########################################################################### Publish
cd $cur_path/$patch_dir
cp $next_ver.zip $cur_path/$new_ver_dir/

cd $cur_path/$new_ver_dir/
#md5sum all.zip $next_ver.zip > md5.txt
md5sum *.zip > md5.txt









