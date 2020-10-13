npx create-nx-workspace@latest allors --preset=empty --cli=nx --nx-cloud=false

cd allors

npm install -D @nrwl/angular
npm install -D @nrwl/react
npm install -D @nrwl/gatsby

npx nx g @nrwl/angular:application angular/app --e2eTestRunner=none
npx nx g @nrwl/angular:application angular/material/app --e2eTestRunner=none
npx nx g @nrwl/gatsby:app gatsby --e2eTestRunner=none

npx nx g @nrwl/workspace:library meta/system
npx nx g @nrwl/workspace:library meta/core
npx nx g @nrwl/workspace:library meta/custom
npx nx g @nrwl/workspace:library meta/generated

npx nx g @nrwl/workspace:library protocol/system

npx nx g @nrwl/workspace:library data/system

npx nx g @nrwl/workspace:library domain/system
npx nx g @nrwl/workspace:library domain/custom
npx nx g @nrwl/workspace:library domain/generated

npx nx g @nrwl/workspace:library promise/core
npx nx g @nrwl/workspace:library promise/custom

npx nx g @nrwl/workspace:library angular/core
npx nx g @nrwl/workspace:library angular/custom
npx nx g @nrwl/workspace:library angular/services/core
npx nx g @nrwl/workspace:library angular/material/core
npx nx g @nrwl/workspace:library angular/material/custom
npx nx g @nrwl/workspace:library angular/material/services/core

npx nx g @nrwl/workspace:library gatsby/source/core
npx nx g @nrwl/workspace:library gatsby/source/custom




