#!/bin/bash

if [ $# -eq 0 ]
  then
    echo Enter arguments:
    read arguments
    dotnet run --project Database/Commands -- $arguments
  else
    dotnet run --project Database/Commands -- "$@"
fi
