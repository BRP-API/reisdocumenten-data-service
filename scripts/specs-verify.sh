#!/bin/bash

EXIT_CODE=0

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
                -p UnitTest \
                > /dev/null
if [ $? -ne 0 ]; then EXIT_CODE=1; fi

verify() {
    npx cucumber-js -f json:./test-reports/cucumber-js/reisdocumenten/test-result-$1.json \
                    -f summary:./test-reports/cucumber-js/reisdocumenten/test-result-$1-summary.txt \
                    -f summary \
                    features/$1 \
                    --tags "not @skip-verify" \
                    --world-parameters "$PARAMS"
}

verify "raadpleeg-met-reisdocumentnummer"
verify "zoek-met-burgerservicenummer"