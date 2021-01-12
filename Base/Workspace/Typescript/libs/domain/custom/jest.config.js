module.exports = {
  preset: '../../../jest.preset.js',
  globals: {
    'ts-jest': {
      tsConfig: '<rootDir>/tsconfig.spec.json',
    },
  },
  testEnvironment: 'node',
  transform: {
    '^.+\\.[tj]sx?$': 'ts-jest',
  },
  moduleFileExtensions: ['ts', 'tsx', 'js', 'jsx', 'html'],
  coverageDirectory: '../../../coverage/libs/domain/custom',
  reporters: [
    'default',
    [
      'jest-trx-results-processor',
      {
        outputFile: '../../../artifacts/tests/base.domain.trx',
      },
    ],
  ],
  displayName: 'domain-custom',
};
