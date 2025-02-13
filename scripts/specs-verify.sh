#!/bin/bash

PARAMS="{ \
    \"apiUrl\": \"http://localhost:8000/haalcentraal/api\", \
    \"logFileToAssert\": \"./test-data/logs/reisdocumenten-data-service.json\", \
    \"oAuth\": { \
        \"enable\": false \
    } \
}"

npx cucumber-js -f json:./test-reports/cucumber-js/step-definitions/test-result-zonder-dependency-integratie.json \
                -f summary:./test-reports/cucumber-js/step-definitions/test-result-zonder-dependency-integratie-summary.txt \
                -f summary \
                features/docs \
                --tags "not @integratie" \
                --tags "not @skip-verify"

npx cucumber-js -f json:./test-reports/cucumber-js/reisdocumenten/test-result.json \
                -f summary:./test-reports/cucumber-js/reisdocumenten/test-result-summary.txt \
                -f summary \
                features/raadpleeg-met-reisdocumentnummer \
                features/zoek-met-burgerservicenummer \
                --world-parameters "$PARAMS"
