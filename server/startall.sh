#!/bin/sh
echo 'Start 127.0.0.1'
ssh 127.0.0.1 /data/ddianle/main.sh startgate /data/gateserver/ /data/billserver/ /data/alllogs/
echo 'Start 127.0.0.1'
ssh 127.0.0.1 /data/ddianle/main.sh startbill /data/gateserver/ /data/billserver/ /data/alllogs/
echo 'Start 127.0.0.1'
ssh 127.0.0.1 /data/ddianle/main.sh startgame /data/gateserver/ /data/billserver/ /data/alllogs/ /data/hotdance
#ssh 127.0.0.1 /data/ddianle/main.sh startgm /data/gmserver/
