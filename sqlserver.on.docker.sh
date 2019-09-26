#!/bin/sh

docker rm --force allors

sudo docker pull mcr.microsoft.com/mssql/server:2017-latest

sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Allors123" \
   -p 1433:1433 --name allors \
   -d mcr.microsoft.com/mssql/server:2017-latest

sudo docker exec -it allors /opt/mssql-tools/bin/sqlcmd \
   -S localhost -U SA -P "Allors123" \
   -Q 'CREATE DATABASE core; CREATE DATABASE base; CREATE DATABASE adapters'



