#!/bin/sh
if [ "$1" = "" ];then
  echo 'Usage: ./synctimeall.sh 2013-9-1 18:00:00'
  exit 1
fi
ssh 127.0.0.1 /data/ddianle/main.sh synctime "$1"
