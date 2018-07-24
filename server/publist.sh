#!/bin/sh

if [ ! -n "$1" ] || [ ! -n "$2" ] || [ ! -n "$3" ] || [ ! -n "$4" ] || [ ! -n "$5" ];then
  echo "Usage:./publish.sh apkv  resv  allresv  ismaintain  isopen"
  exit 0
fi

if [ ! -n "$6" ];then
  xmlfile="version8.xml"
else
  xmlfile="$6"
fi

#配置区域
pname=(test)
#pname=(ddianle ddianle1 downjoy ucweb wl91 miui qihoo baidu kuwo 4399 ttdt kingsoft oppo wdj anzhi lenovo alibaba ddianle2 ddianle501 ddianle502 ddianle503 ddianle504 ddianle505 ddianle506 ddianle507 ddianle508 ddianle509 ddianle510 ddianle511 ddianle512 ddianle513 ddianle514 ddianle515 ddianle516 ddianle517 ddianle518 ddianle519 ddianle520 ddianle521 ddianle522 ddianle523 ddianle524 ddianle525 ddianle526 ddianle528 ddianle529 alibaba_1 alibaba_2 alibaba_3 alibaba_4 alibaba_5 alibaba_6 sy37)
#apkname=(ddl ddianle dj uc 91 mi 360 baidu kuwo 4399 ttdt king oppo wdj anzhi lenovo sina ali 501 502 503 504 505 506 507 508 509 510 511 512 513 514 515 516 517 518 519 520 521 522 523 524 525 526 528 529 al_1 al_2 al_3 al_4 al_5 al_6 sy37) 
apkname=(test)
iver=1
apkresip="10.4.9.176"
uploadip="test4.ddianle.com"
downloadip="test4.ddianle.com"
gateip="test4.ddianle.com"
sourcerip="10.4.9.176 "
sectionname=(内部测试区梁亮)
sectionip=(test4.ddianle.com)


index=0
index2=0
allresmd5=$(ssh "$sourcerip" md5sum "/data/tempdata/test/res/all.zip" | cut -d ' ' -f1|sort)
for((h=1;h<=$2;h++))
do
    md5res[$h]=$(ssh "$sourcerip" md5sum "/data/tempdata/test/res/"$h".zip" | cut -d ' ' -f1|sort)
    echo "Res ["$h"] is ok!"
done

for pdir in ${pname[*]}
do
  echo $pdir"   begin......"
  if [ ! -d "$pdir" ];then
    mkdir $pdir
  fi
  cd $pdir
  rm $xmlfile -f
  echo "<?xml version='1.0' encoding='UTF-8'?>" >> $xmlfile
  echo "<webgis>" >> $xmlfile
  echo "<ismaintain>"$4"</ismaintain>" >> $xmlfile
  echo "<strmaintain>例行维护中，更多信息请关注恋舞OL官方微博！</strmaintain>" >> $xmlfile
  echo "<isopen>"$5"</isopen>" >> $xmlfile
  echo "<stropen>服务器暂未开放，请稍后再试！</stropen>" >> $xmlfile

  echo "<apkurl>http://"$apkresip"/allsource/test/apk/L5_"${apkname[$index]}".apk</apkurl>" >> $xmlfile
  
  apkmd5=$(ssh "$sourcerip" md5sum "/data/tempdata/test/apk/L5_"${apkname[$index]}".apk" | cut -d ' ' -f1|sort)
  echo "<md5apk>"$apkmd5"</md5apk>" >> $xmlfile

  echo "<apkver>"$1"</apkver>" >> $xmlfile

  echo "<resver>"$2"</resver>" >> $xmlfile
  echo "<allresver>"$3"</allresver>" >> $xmlfile
  echo "<resurl>http://"$apkresip"/allsource/test/res/</resurl>" >> $xmlfile

  echo "<md5allres>"$allresmd5"</md5allres>" >> $xmlfile

  for((j=1;j<=$2;j++))
  do
    echo "<md5res"$j">"${md5res[$j]}"</md5res"$j">" >> $xmlfile
  done

  echo "<dirbase>/xuanqu/lwts/</dirbase>" >> $xmlfile

  sectionnum=${#sectionname[@]} 
  echo "<section>""$sectionnum""</section>" >> $xmlfile
  index2=0
  for psec in ${sectionname[*]}
  do
    let "ncurnum=$index2+1"
    echo "<sectionname""$ncurnum"">""$psec""</sectionname""$ncurnum"">" >> $xmlfile
    echo "<sectionip""$ncurnum"">""${sectionip[$index2]}""</sectionip""$ncurnum"">" >> $xmlfile
    let index2++
  done
 
  echo "<iver>""$iver""</iver>" >> $xmlfile

  echo "<uploadip>"$uploadip"</uploadip>" >> $xmlfile
  echo "<uploadport>8887</uploadport>" >> $xmlfile
  echo "<downloadip>""$downloadip""</downloadip>" >> $xmlfile
  echo "<downloadport>8887</downloadport>" >> $xmlfile

  echo "<serverip>"$gateip"</serverip>" >> $xmlfile
  echo "</webgis>" >> $xmlfile

  cd ..
  let index++
  echo "success"
  echo -e "\n"
done

