@ECHO OFF
:: Installs all Nuget packages for all Projects, Recursively.
:: Ignores Resharper folders
:: DOS Help - http://www.dostips.com/forum/viewtopic.php?f=3&t=431
SET NUGET="Nuget.exe"
SET SOLUTIONDIR=..\src

ECHO.
ECHO Finding Projects in Solution... (saving to ProjectsUsingNuget.log.txt)
ECHO ------------------------------
DIR "%SOLUTIONDIR%\packages.config" /b /s | Find /I /V "_ReSharper" > ProjectsUsingNuget.log.txt

FOR /F "delims=" %%A In (ProjectsUsingNuget.log.txt) Do (
ECHO.
ECHO Updating Packages for Project:
ECHO ------------------------------
ECHO %%A
@ECHO ON
%NUGET% install "%%A" -OutputDirectory "%SOLUTIONDIR%\packages"
@ECHO OFF
)