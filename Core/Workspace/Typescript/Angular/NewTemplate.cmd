@echo off

cd \temp

DEL /F/Q/S material > NUL
RMDIR /Q/S material

CALL ng new angular --routing=true --style=scss --strict

pause