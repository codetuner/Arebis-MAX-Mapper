@ECHO OFF
GIT add .
SET /p msg=Comment ? 
GIT commit -m "%msg%"
GIT push