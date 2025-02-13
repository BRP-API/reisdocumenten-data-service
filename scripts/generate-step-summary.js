const fs = require('fs');
const path = require('path');
const { processFile } = require('./process-cucumber-file');

const outputFile = path.join(__dirname, './../test-reports/cucumber-js/reisdocumenten/step-summary.txt');
fs.writeFileSync(outputFile, '', 'utf8');

const fileMap = new Map([
    ["./../test-reports/cucumber-js/step-definitions/test-result-zonder-dependency-integratie-summary.txt", "docs (zonder integratie)"],
    ["./../test-reports/cucumber-js/reisdocumenten/test-result-raadpleeg-met-reisdocumentnummer-summary.txt", "raadpleeg met reisdocumentnummer"],
    ["./../test-reports/cucumber-js/reisdocumenten/test-result-zoek-met-burgerservicenummer-summary.txt", "zoek met burgerservicenummer"],
]);

fileMap.forEach((caption, filePath) => {
    processFile(path.join(__dirname, filePath), outputFile, caption);
});