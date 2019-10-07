@echo off

cd \temp

DEL /F/Q/S intranet > NUL
RMDIR /Q/S intranet

CALL ng new intranet --routing=true --style=scss
cd intranet
CALL ng add @angular/material --defaults=true

pause