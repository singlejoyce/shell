#!/bin/sh
if [ "$1" = "" ] || [ "$2" = "" ];then
  echo 'Usage: ./updateall.sh game|gate|bill|large|gm file'
  exit 1
fi

game_server=(127.0.0.1)
gate_server=(127.0.0.1)
bill_server=(127.0.0.1)
gm_server=(127.0.0.1)
largebill_server=(127.0.0.1)

if [ "$1" = "game" ];then
	for game_s in ${game_server[@]}
	do
		echo "Update $game_s game"
		/usr/bin/rsync /data/ddianle/server/"$2"  "$game_s":/data/hotdance
			for i in `seq 4`
			do
				
				if [ "$i" -eq 1 ];then
					ssh "$game_s" tar zxvf /data/hotdance/"$2" -C /data/hotdance
				else
					ssh "$game_s" tar zxvf /data/hotdance/"$2" -C /data/hotdance"$i"
				fi
			done
		echo "Update $game_s game successfull!"
	done
fi

if [ "$1" = "gate" ];then
        for gate_s in ${gate_server[@]}
        do
                echo "Update $gate_s gate"
                /usr/bin/rsync /data/ddianle/server/"$2"  "$gate_s":/data/gateserver
                echo "Update $gate_s gate successfull!"
        done    
fi

if [ "$1" = "bill" ];then
        for bill_s in ${bill_server[@]}
        do
                echo "Update $bill_s bill"
                /usr/bin/rsync /data/ddianle/server/"$2"  "$bill_s":/data/billserver
                echo "Update $bill_s bill successfull!"
        done    
fi

if [ "$1" = "large" ];then
        for largebill_s in ${largebill_server[@]}
        do
                echo "Update $largebill_s largebill"
                /usr/bin/rsync /data/ddianle/server/"$2"  "$largebill_s":/data/largebill
                echo "Update $largebill_s largebill successfull!"
        done
fi

if [ "$1" = "gm" ];then
        for gm_s in ${gm_server[@]}
        do
                echo "Update $gm_s gm"
                /usr/bin/rsync /data/ddianle/server/"$2"  "$gm_s":/data/gmserver
                echo "Update $gm_s gm successfull!"
        done
fi

