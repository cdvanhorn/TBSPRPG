#!/bin/bash

#echo -n "enter directory where packages should be linked: "
#read directory
cd $1
mkdir -p Packages
cd Packages
rm *.nupkg 2> /dev/null
ln ../../TBSPRPG/TbspRpgLib/bin/Release/*.nupkg ./
ln ../../TBSPRPG/TbspRpgLib.Tests/bin/Release/*.nupkg ./
