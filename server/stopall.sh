#!/bin/sh
echo 'Stop 127.0.0.1'
ssh 127.0.0.1 /data/ddianle/main.sh stopgate /data/gateserver/ /data/billserver/ /data/alllogs/
echo 'Stop 127.0.0.1'
ssh 127.0.0.1 /data/ddianle/main.sh stopbill /data/gateserver/ /data/billserver/ /data/alllogs/
echo 'Stop 127.0.0.1'
ssh 127.0.0.1 /data/ddianle/main.sh stopgame /data/gateserver/ /data/billserver/ /data/alllogs/ /data/hotdance
ssh 127.0.0.1 /data/ddianle/main.sh stopgm /data/gmserver/
