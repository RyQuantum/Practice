set CGO_ENABLED=1
set GOARCH=386
set path=%path%;C:\Progra~1\mingw-w64\i686-8.1.0-posix-dwarf-rt_v6-rev0\mingw32\bin
set CC=i686-w64-mingw32-gcc
set CXX=i686-w64-mingw32-g++

go build -buildmode=c-archive main1.go
gcc main1.def main1.a -shared -lwinmm -lWs2_32 -o main1.dll -Wl,--out-implib,main1.lib
