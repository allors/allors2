module.exports = {
  displayName: 'angular-custom',
  preset: '../../../jest.preset.js',
  globals: {
    'ts-jest': {
      tsconfig: '<rootDir>/tsconfig.spec.json',
    },
  },
  transform: {
    '^.+\\.[tj]sx?$': 'ts-jest',
  },
  moduleFileExtensions: ['ts', 'tsx', 'js', 'jsx'],
  coverageDirectory: '../../../coverage/libs/angular/custom',
  reporters: [
    'default',
    [
      'jest-trx-results-processor',
      {
        outputFile: '../../../artifacts/tests/core.angular.trx',
      },
    ],
  ],
  // Allors: sequential with extra time
  maxWorkers: 1,
  testTimeout: 60000,
};
