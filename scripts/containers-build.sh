#!/bin/bash

MODE=$1

if [ "$MODE" = "ci" ]; then
    # gebruik docker compose up -d om te forceren dat de container image wordt aangemaakt in de lokale registry
    docker compose -f .docker/docker-compose-ci.yml up -d
    docker compose -f .docker/docker-compose-ci.yml down
else
    docker compose -f src/docker-compose.yml build
fi
