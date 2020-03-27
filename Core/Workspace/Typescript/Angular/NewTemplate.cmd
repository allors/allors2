@echo off

cd \temp

DEL /F/Q/S angular > NUL
RMDIR /Q/S angular

CALL ng new angular --routing=true --style=scss

pause