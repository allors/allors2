@set PATH=%APPDATA%\npm;C:\Program Files (x86)\nodejs;C:\Program Files\nodejs;%PATH%

cd desktop

call bower install
call typings install

cd ..
