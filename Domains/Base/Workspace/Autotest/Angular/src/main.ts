#! /usr/bin/env node
import * as fs from 'fs';
import { project } from './config';
import { Angular } from './Angular/Angular';

console.log();
console.log('Autotest');
console.log('========');

console.log('- Project');

const angular = new Angular();
angular.Load(project);

fs.mkdirSync('./dist', { recursive: true } as any);
fs.writeFileSync('./dist/angular.json', JSON.stringify(angular));