@echo off
echo _
echo _
echo _
echo Will remove git subrepo references, close this window to cancel
echo _
echo _
echo _
pause
RMDIR ".git" /S /Q
del .gitignore
del gitmoduleexample.zip
del removeSubRepo.bat.meta
del removeSubRepo.bat 
