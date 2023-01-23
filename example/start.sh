#!/usr/bin/env bash
docker-compose down

git pull

docker build -f ../src/OposedApi/Dockerfile --force-rm -t oposed_api ../src/
docker build -f ../src/Oposed/Dockerfile --force-rm -t oposed_web ../src/

docker-compose up -d