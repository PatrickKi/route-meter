@ECHO OFF
ECHO Performing clean up...

SET ROOT=%~dp0

RMDIR /s /q "%ROOT%RouteMeter\bin"
ECHO Deleted bin...
RMDIR /s /q "%ROOT%RouteMeter\obj"
ECHO Deleted obj...

pause