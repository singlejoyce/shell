#!/bin/sh
echo '[127.0.0.1]'
ssh 127.0.0.1 ps -ef | grep 'Gate.jar' | grep -v 'grep'
ssh 127.0.0.1 ps -ef | grep 'Bill.jar' | grep -v 'grep'
ssh 127.0.0.1 netstat -nal | grep 7750 | wc -l
ssh 127.0.0.1 netstat -nal | grep 9999 | wc -l
ssh 127.0.0.1 date
echo '[127.0.0.1]'
ssh 127.0.0.1 ps -ef | grep "/data/hotdance" | grep -v "grep"
ssh 127.0.0.1 date
echo '[127.0.0.1]'
ssh 127.0.0.1 ps -ef | grep 'GMServer' | grep -v 'grep'
ssh 127.0.0.1 netstat -nal | grep 7777 | wc -l
ssh 127.0.0.1 date
