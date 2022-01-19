#!/usr/bin/env bash

docker build -f "RoomAndResourcesSchedulerApi/Dockerfile" --force-rm -t roomandresourcesscheduler_api .
docker build -f "RoomAndResourcesScheduler/Dockerfile" --force-rm -t roomandresourcesscheduler_web .

docker-compose up -d