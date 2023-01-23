#!/usr/bin/env bash
docker-compose down

docker build -f "../OposedApi/Dockerfile" --force-rm -t oposed_api ../
docker build -f "../Oposed/Dockerfile" --force-rm -t oposed_web ../

docker-compose up -d

start "" http://localhost:8080