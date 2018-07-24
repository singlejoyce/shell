@echo off
for /f "delims=" %%a in (song.txt) do (
    copy /y "E:\Resource_N1_Unity4.7.2\Res\Icon\%%a.texture" "查找后需要存放的地址"
)