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

verify() {
    npx cucumber-js -f json:./test-reports/cucumber-js/reisdocumenten/test-result-$1.json \
                    -f summary:./test-reports/cucumber-js/reisdocumenten/test-result-$1-summary.txt \
                    -f summary \
                    features/$1 \
                    --world-parameters "$PARAMS"
}

verify "raadpleeg-met-reisdocumentnummer"
verify "zoek-met-burgerservicenummer"