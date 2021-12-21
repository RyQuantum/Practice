@echo off

rd bin /s /q
mkdir bin\32
mkdir bin\64

set fn=main
set gn=main.go
set dn=main.def

::设置目标平台(32位)
set CGO_ENABLED=1
set GOARCH=386
set path=%path%;C:\Progra~1\mingw-w64\i686-8.1.0-posix-dwarf-rt_v6-rev0\mingw32\bin
set CC=i686-w64-mingw32-gcc
set CXX=i686-w64-mingw32-g++
::编译32位静态库

go build -buildmode=c-archive -o bin\32\%fn%.a %gn%
gcc %dn% bin\32\%fn%.a -shared -lwinmm -lWs2_32 -o bin\32\%fn%.dll -Wl,--out-implib,bin\32\%fn%.lib
