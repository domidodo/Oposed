@echo off

:: Check if docker is running
call docker container ls
IF /I "%ERRORLEVEL%" NEQ "0" GOTO dockerDontRun
cls

docker-compose down

docker build -f "../src/OposedApi/Dockerfile" --force-rm -t oposed_api ../src/
docker build -f "../src/Oposed/Dockerfile" --force-rm -t oposed_web ../src/

docker-compose up -d
GOTO end


start "" http://localhost:8080

:dockerDontRun
cls
echo ------------------------------------------------
echo DOCKER DAEMON IS NOT RUNNING                    
echo Please start the docker daemon and try again.   
echo ------------------------------------------------
echo.
pause

:end