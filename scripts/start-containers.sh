#!/bin/bash

docker compose -f .docker/db.yml -f .docker/reisdocumenten-data-service.yml up -d