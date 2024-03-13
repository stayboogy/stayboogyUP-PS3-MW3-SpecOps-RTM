copy README.md Release\Readme.txt
cd Release
ren StayboogyUP.exe stayboogyUP.exe
tar.exe -cf stayboogyUP-rtm-last.zip CCAPI.dll CCAPI-install.cmd PS3Lib.dll stayboogyUP.exe Readme.txt
del CCAPI.dll 
del CCAPI-install.cmd
del PS3Lib.dll
del stayboogyUP.exe
cd ..\
start cleanup.cmd
exit

