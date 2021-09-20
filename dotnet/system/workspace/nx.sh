npx create-nx-workspace@latest allors --preset=empty --cli=nx --nx-cloud=false

cd allors

npm install -D jest-chain
npm install -D jest-extended
npm install -D jest-trx-results-processor

npx nx g @nrwl/workspace:library shared/system
npx nx g @nrwl/workspace:library shared/tests
npx nx g @nrwl/workspace:library protocol/json/system

npx nx g @nrwl/workspace:library workspace/meta/system
npx nx g @nrwl/workspace:library workspace/meta/lazy/system
npx nx g @nrwl/workspace:library workspace/meta/lazy/tests

npx nx g @nrwl/workspace:library workspace/domain/system
npx nx g @nrwl/workspace:library workspace/domain/json/system

