#!/bin/bash

if [ $# -eq 0 ]
  then
    dotnet run --project Database/Commands -- --help
    echo Enter arguments:
    read arguments
    dotnet run --project Database/Commands -- $arguments
  else
    dotnet run --project Database/Commands -- "$@"
fi
