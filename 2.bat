@echo off

set /p Anipath=please enter animation file path:
set /p outpath=please enter output file path:


cd /d %Anipath%

DIR %Anipath%\*.*  /B >%outpath%\list.txt

REM 打印出匹配的行
REM for /f "delims=." %%i in (%outpath%\list.txt) do （findstr /s "%%i" E:\project_n1\unity_n1\Res\Animations\Controller\* >>result.txt）

for /f "delims=." %%i in (%outpath%\list.txt) do findstr /s "%%i" %Anipath%\Controller\* && echo %%i >> %outpath%\find.txt || echo %%i >> %outpath%\notfind.txt


REM for /f %%i in (%outpath%\notfind.txt) do del /f /s /q %Anipath%\%%i.assetbundle

pause;
