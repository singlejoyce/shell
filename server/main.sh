#!/bin/sh

markend="/"

getgatepid(){
  ps ax | grep "Gate.jar" | grep -v "grep" | awk '{if(NR==1) {print $1}}'
}

getbillpid(){
 ps ax | grep "Bill.jar" | grep -v "grep" | awk '{if(NR==1) {print $1}}'
}

getcurdate(){
  date | awk '{print $6 $2 $3 $4} ' | sed "s/://g"
}

startgame(){
  ulimit -c unlimited
  "$gamedir"/bin/debug_hotdance start
}

startgate(){
  ulimit -HSn 102400
  cd "$gatedir"
  nohup ./rungate.sh > nohup.out 2>&1 &
  echo "Start GateServer OK!"
}

startbill(){
  ulimit -HSn 102400
  cd "$billdir"
  nohup ./runbill.sh > nohup.out 2>&1 &
  echo "Start BillServer OK!"
}

startgm(){
  cd "$gmdir"
  nohup ./GMServer > GMServer.output 2>&1 &
  echo "Starting GMServer OK!"
}

stopgame(){
  "$gamedir"/bin/debug_hotdance stop
}

stopgate(){
  gatepid=$(getgatepid)

  if [ "$gatepid" = "" ];then
    echo "GateServer is not running!"
  else
    kill -9 "$gatepid"
    echo "Close GateServer Ok!"
  fi
}

stopbill(){
  billpid=$(getbillpid)

  if [ "$billpid" = "" ];then
    echo "BillServer is not running!"
  else
    kill -9 "$billpid"
    echo "Close BillServer Ok!"
  fi 
}

stopgm(){
  pkill -9 GMServer
  echo "Stop GMServer OK"
}

updategame(){
  for((i=1;i<=4;i++))
  do
    if [ "$i" -eq 1 ];then
      pdir="$gamedir""$markend"
    else
      pdir="$gamedir""$i""$markend"
    fi

    if [ ! -d "$pdir" ];then
      mkdir -p "$pdir"
    fi
  
    cd "$pdir"
    rm "$ver" -f
  
    if [ "$i" -eq 1 ];then
      scp "$sourceip":"$sourcedir""$markend""$ver" "$pdir"
    else
      cp "$gamedir""$markend""$ver" "$pdir"
    fi

    tar zxvf "$ver"
  done
}

updategate(){
  if [ ! -d "$gatedir" ];then
    mkdir -p "$gatedir"
  fi

  cd "$gatedir"
  rm "$gatefile" -f

  scp "$sourceip":"$sourcedir""$markend""$gatefile" "$gatedir"
  endtag=$(echo "$gatefile" | grep ".tgz")
  if [ "$endtag" = "" ];then
    echo "update OK"
  else
    tar zxvf "$gatefile"
    echo "tar OK"
  fi
}

updatebill(){
  if [ ! -d "$billdir" ];then
    mkdir -p "$billdir"
  fi

  cd "$billdir"
  rm "$billfile" -f

  scp "$sourceip":"$sourcedir""$markend""$billfile" "$billdir"
  endtag=$(echo "$billfile" | grep ".tgz")
  if [ "$endtag" = "" ];then
    echo "update OK"
  else
    tar zxvf "$billfile"
    echo "tar OK"
  fi
}

updategm(){
  if [ ! -d "$gmdir" ];then
    mkdir -p "$gmdir"
  fi

  cd "$gmdir"
  rm "$gmfile" -f

  scp "$sourceip":"$sourcedir""$markend""$gmfile" "$gmdir"
  endtag=$(echo "$gmfile" | grep ".tgz")
  if [ "$endtag" = "" ];then
    echo "update OK"
  else
    tar zxvf "$gmfile"
    echo "tar OK"
  fi
}

clear(){
  curdate=$(getcurdate)
  if [ ! -d "$logdir" ];then
    mkdir -p "$logdir"
  fi
  
  cd "$logdir"

  if [ ! -d "$curdate" ];then
    mkdir "$curdate"
  fi

  for((i=1;i<=4;i++))
  do
    if [ "$i" -eq 1 ];then
      pdir="$gamedir""$markend"
    else
      pdir="$gamedir""$i""$markend"
    fi
    
    if [ -d "$pdir" ];then
      cd "$pdir"
      cp logs/* ../alllogs/"$curdate" -rf
      rm * -rf
    fi
  done
}

synctime(){
  date -s "$timenow"
}

# see how we were called.

case "$1" in
  startgame)
    if [ "$2" = "" ] || [ "$3" = "" ] || [ "$4" = "" ] || [ "$5" = "" ];then
      echo "Usage:./main.sh startgame gatedir billdir logdir gamedir"
      exit 1
    fi
    gatedir="$2"
    billdir="$3"
    logdir="$4"
    gamedir="$5"
    startgame
    ;;
  startgate)
    if [ "$2" = "" ] || [ "$3" = "" ] || [ "$4" = "" ];then
      echo "Usage:./main.sh startgate gatedir billdir logdir"
      exit 1
    fi
    gatedir="$2"
    billdir="$3"
    logdir="$4"
    startgate
    ;;
  startbill)
    if [ "$2" = "" ] || [ "$3" = "" ] || [ "$4" = "" ];then
      echo "Usage:./main.sh startbill gatedir billdir logdir"
      exit 1
    fi
    gatedir="$2"
    billdir="$3"
    logdir="$4"
    startbill
    ;;
  startgm)
    if [ "$2" = "" ];then
      echo "Usage:./main.sh startgm gmdir"
      exit 1
    fi
    gmdir="$2"
    startgm
    ;;
  stopgame)
    if [ "$2" = "" ] || [ "$3" = "" ] || [ "$4" = "" ] || [ "$5" = "" ];then
      echo "Usage:./main.sh stopgame gatedir billdir logdir gamedir"
      exit 1
    fi
    gatedir="$2"
    billdir="$3"
    logdir="$4"
    gamedir="$5"
    stopgame
    ;;
  stopgate)
    if [ "$2" = "" ] || [ "$3" = "" ] || [ "$4" = "" ];then
      echo "Usage:./main.sh stopgate gatedir billdir logdir"
      exit 1
    fi
    gatedir="$2"
    billdir="$3"
    logdir="$4"
    stopgate
    ;;
  stopbill)
    if [ "$2" = "" ] || [ "$3" = "" ] || [ "$4" = "" ];then
      echo "Usage:./main.sh stopbill gatedir billdir logdir"
      exit 1
    fi
    gatedir="$2"
    billdir="$3"
    logdir="$4"
    stopbill
    ;;
  stopgm)
    stopgm
    ;;
  clear)
    if [ "$2" = "" ] || [ "$3" = "" ] || [ "$4" = "" ] || [ "$5" = "" ];then
      echo "Usage:./main.sh clear gatedir billdir logdir"
      exit 1
    fi
    gatedir="$2"
    billdir="$3"
    logdir="$4"
    gamedir="$5"
    clear
    ;;
  synctime)
    if [ "$2" = "" ];then
      echo "Usage:./main.sh synctime timenow"
      exit 1
    fi
    timenow="$2"
    synctime
    ;;
  updategame)
    if [ "$2" = "" ] || [ "$3" = "" ] || [ "$4" = "" ] || [ "$5" = "" ] || [ "$6" = "" ] || [ "$7" = "" ] || [ "$8" = "" ];then
      echo "Usage:./main.sh updategame *.tgz sourceip sourcedir gatedir billdir logdir" 
      exit 1
    fi
    ver="$2"
    sourceip="$3"
    sourcedir="$4"
    gatedir="$5"
    billdir="$6"
    logdir="$7"
    gamedir="$8"
    updategame
    ;;
  updategate)
    if [ "$2" = "" ] || [ "$3" = "" ] || [ "$4" = "" ] || [ "$5" = "" ] || [ "$6" = "" ] || [ "$7" = "" ];then
      echo "Usage:./main.sh updategate Gate.tgz sourceip sourcedir gatedir billdir logdir"
      exit 1
    fi
    gatefile="$2"
    sourceip="$3"
    sourcedir="$4"
    gatedir="$5"
    billdir="$6"
    logdir="$7"
    updategate
    ;;
  updatebill)
    if [ "$2" = "" ] || [ "$3" = "" ] || [ "$4" = "" ] || [ "$5" = "" ] || [ "$6" = "" ] || [ "$7" = "" ];then
      echo "Usage:./main.sh updatebill Bill.tgz sourceip sourcedir gatedir billdir logdir"
      exit 1
    fi
    billfile="$2"
    sourceip="$3"
    sourcedir="$4"
    gatedir="$5"
    billdir="$6"
    logdir="$7"
    updatebill
    ;;
  updategm)
    if [ "$2" = "" ] || [ "$3" = "" ] || [ "$4" = "" ] || [ "$5" = "" ];then
      echo "Usage:./main.sh updategm gmtool.tgz sourceip sourcedir gmdir"
      exit 1
    fi
    gmfile="$2"
    sourceip="$3"
    sourcedir="$4"
    gmdir="$5"
    updategm
    ;;
  *)
    echo "Usage:./main.sh startgame | startgate | startbill | stopgame | stopgate | stopbill | updategame | clear"
    ;;
esac
