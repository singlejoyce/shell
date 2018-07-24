#!/bin/sh

if [ ! -n "$1" ];then
  echo "Usage:./build.sh project's path"
  exit 0
fi
echo "start build res in $1..."
date

#/Applications/Unity/Unity.app/Contents/MacOS/Unity
UNITY_PATH=$1

#project's path
PROJECT_PATH=$2

#RES_OUT_PATH
RES_OUT_PATH=$3 

#builditems
BUILD_ITEMS=$4

hasItem=`echo $BUILD_ITEMS|grep -E '1|0'|wc -l`

if [ $hasItem = 1 ] 
then 
echo "building ui..."
#$UNITY_PATH -quit -batchmode  -executeMethod GenerateUI.GenerateAllUIScene -projectPath $PROJECT_PATH -logFile buildUI.log
date 
fi

hasItem=`echo $BUILD_ITEMS|grep -E '2|0'|wc -l`

if [ $hasItem = 1 ] 
then 
echo "building atlas..."
#$UNITY_PATH -quit -batchmode -executeMethod GenerateAtlas.Generate_Atlas -projectPath $PROJECT_PATH -logFile buildAtlas.log
date 
fi

hasItem=`echo $BUILD_ITEMS|grep -E '3|0'|wc -l`

if [ $hasItem = 1 ] 
then 
echo "building stage..."
#$UNITY_PATH -quit -batchmode -executeMethod GenerateStage.Generate_Stage -projectPath $PROJECT_PATH -logFile buildStage.log
date 
fi

###########################################deleting the assetbundle is not necessary
#echo  "delete Animations/*.assetbundle"
#rm -rf /Applications/$1/Res/Animations/*.assetbundle
hasItem=`echo $BUILD_ITEMS|grep -E '4|0'|wc -l`

if [ $hasItem = 1 ] 
then 
echo "building animation..."
#$UNITY_PATH -quit -batchmode -executeMethod GenerateAnimation.GenerateAnimations -projectPath $PROJECT_PATH -logFile buildAnimation.log
date 
fi

hasItem=`echo $BUILD_ITEMS|grep -E '5|0'|wc -l`

if [ $hasItem = 1 ] 
then 
echo "building bones..."
#$UNITY_PATH -quit -batchmode -executeMethod GenerateBone.GenerateBones -projectPatch $PROJECT_PATH -logFile buildBone.log
date 
fi

#echo "building effect..."
#/Applications/Unity/unity.app/contents/MacOS/unity -quit -batchmode -executeMethod GenerateEffect.GenerateSpecialEffect -projectPath "/Applications/$1" -logFile buildEffect.log
#date

echo "build all res success."

# cp -apX $PROJECT_PATH/Res $RES_OUT_PATH

# echo "copy res success."