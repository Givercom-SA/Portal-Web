@echo off

setlocal EnableDelayedExpansion

@echo off
for /F "usebackq tokens=1,2 delims==" %%i in (`wmic os get LocalDateTime /VALUE 2^>NUL`) do if '.%%i.'=='.LocalDateTime.' set ldt=%%j
set ldt=%ldt:~2,2%%ldt:~4,2%%ldt:~6,2%

set datestr=%ldt%

for /f %%I in (build.version) do (     
    set version=%%I
    set build=!version:~10,2!
	set build=!build:_=.!
    set release=!version:~0,3!
	set release=!release:_=.!
	set dateFile=!version:~3,6!
	set buildIncrement=1
	if !datestr!==!dateFile! (set /A buildIncrement=!build!+1) 
	if NOT !datestr!==!dateFile! (set /A buildIncrement=1)
	set final=!release!!datestr!.!buildIncrement!
    echo !final!
	echo !final! > build.version
	goto next
)
:next
