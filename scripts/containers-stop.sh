#!/bin/bash

MODE=$1

if [ "$MODE" = "ci" ]; then
    docker compose -f .docker/db-ci.yml down
else
    docker compose -f .docker/db.yml down
fi

docker compose -f .docker/reisdocumenten-data-service.yml down