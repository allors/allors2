@echo off

cd \temp

DEL /F/Q/S material > NUL
RMDIR /Q/S material

CALL ng new material --routing=true --style=scss
cd material
CALL ng add @angular/material --defaults=true

pause