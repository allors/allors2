@echo off

rem Use following command to create submodule 
rem git submodule add -b master https://github.com/dipu/excel.git excel

rem Use following command to update all submodules
rem git submodule update --init --recursive

cd excel
git checkout master
git pull

cd ..
git add excel

@pause


