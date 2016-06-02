@ECHO OFF

SETLOCAL

SET SZEXE=7za.exe

CALL :MakeZip "Max Empty Master Generation Template"
CALL :MakeZip "Max Empty SubTemplate"
CALL :MakeZip "Max Generated Contracts Library"
CALL :MakeZip "Max Generated Portable Contracts Library"
CALL :MakeZip "Max GetSourcePropertyName Mapper Template"
CALL :MakeZip "Max Identifier Based ObjectSource Mapper Template"
CALL :MakeZip "Max IEntityWithKey Contracts Template"
CALL :MakeZip "Max IExtensibleDataObject Contracts Template"
CALL :MakeZip "Max Mapper Generator Templates Set"
CALL :MakeZip "Max Model Validation Contracts Template"
CALL :MakeZip "Max Select Projections Mapper Template"
CALL :MakeZip "Max StringConverter Contracts Template"
EXIT /B 0


:MakeZip
DEL "%~dp0%~1.zip"
"%SZEXE%" a -r -tzip "%~dp0%~1.zip" "%~dp0%~1\*"
EXIT /B 0