@echo off

cd domain
call npm run link

cd ../generated
call npm run link

cd ../angular
call npm run link

cd ../material
call npm run link

cd ../covalent
call npm run link
