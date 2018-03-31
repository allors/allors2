const { SpecReporter } = require('jasmine-spec-reporter');
var TeamCityReporter = require('jasmine-reporters').TeamCityReporter;

exports.config = {
  allScriptsTimeout: 11000,
  specs: [
    './out-tsc/e2e/**/*.e2e-spec.js'
  ],
  capabilities: {
    'browserName': 'chrome'
  },
  directConnect: true,
  baseUrl: 'http://localhost:4200/',
  framework: 'jasmine',
  jasmineNodeOpts: {
    showColors: true,
    defaultTimeoutInterval: 2147483647, // 2^31-1
    print: function () { }
  },
  onPrepare() {
    require('ts-node').register({
      project: 'e2e/tsconfig.e2e.json',
    });
    jasmine.getEnv().addReporter(new TeamCityReporter());
  }
};