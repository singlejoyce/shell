#!/bin/bash

######### configuration
serverUrl="svn://172.16.1.237/backup/SmartPhone_P1/server"
dataUrl="svn://172.16.1.237/backup/Resource_P1_IOS/Data"
productionHome="/opt/smartphone"
serverHome="$productionHome/server_ios_p1"
dataHome="$serverHome/server/data"
detaiLog="$productionHome/build_detail_ios_p1.log"
summaryLog="$productionHome/build_summary_ios_p1.log"


######### ENV setting
export LANG=zh_CN.GB18030

######### checkout source code and data
rm -rf $serverHome
svn export $serverUrl $serverHome --username build --password dailybuild

#rm -rf $dataHome
svn export --force $dataUrl $dataHome --username build --password dailybuild


######### compile and packing
cd $serverHome
chmod +x server/bin/*

bash make-ios.sh > $detaiLog 2>&1

######### result log
errorCount=$(grep error $detaiLog | wc -l)
warningCount=$(grep warn $detaiLog | wc -l)

echo "<html><header><title>daily build</title><header><body>" > $summaryLog
if [ $errorCount -gt 0 ]; then
    echo "STATUS: <strong><font color=red>FAILED</font></strong><br/>" >> $summaryLog
else
    echo "STATUS: <strong><font color=green>PASS</font></strong><br/>" >> $summaryLog
fi
echo "WARNING: <strong><font color=purple>$warningCount</font></strong><br/>" >> $summaryLog
echo "<pre>" >> $summaryLog
echo "============= daily build summary =============" >> $summaryLog
date "+%F %T" >> $summaryLog
echo "" >> $summaryLog

grep -v "Compile" $detaiLog >> $summaryLog

echo "" >> $summaryLog
echo "===============================================" >> $summaryLog
echo "</pre></body></html>" >> $summaryLog

#killall -9 daily-build.sh
#killall -9 nc
#
#while true; do
#    nohup nc -l 8080 < $summaryLog >> /dev/null
#done
#
# while true; do  nohup nc -l 8080 < /opt/yangt/build-summary.log >> /dev/null; done &
#
# crontab
# 0 * * * * /opt/yangt/daily-build.sh >> /tmp/daily-build.log
