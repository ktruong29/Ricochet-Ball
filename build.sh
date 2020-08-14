#!/bin/bash
#/*******************************************************************************
# *Author: Kien Truong
# *Assignment 3
# *CPSC 223N
#******************************************************************************/

echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile RicochetBall.cs to create the file: TravelingBall.dll
mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:RicochetBall.dll RicochetBall.cs

echo Link the previously created dll file to create an executable file.
mcs -r:System -r:System.Windows.Forms -r:RicochetBall.dll -out:Ricochet.exe main.cs

echo View the list of files in the current folder
ls -l

echo Run the Assignment 3 program.
./Ricochet.exe

echo The script has terminated.
