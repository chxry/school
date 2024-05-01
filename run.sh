#!/bin/sh
set -e
csc "$1.cs"
if [[ "$OSTYPE" == "darwin"* ]]; then
  WINEDEBUG=-all wine "$1.exe"
else
  mono "$1.exe"
fi
