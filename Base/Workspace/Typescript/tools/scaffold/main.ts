#! /usr/bin/env node
import * as fs from 'fs';
import { project } from './config';

console.log();
console.log('Scaffold');
console.log('========');

console.log('-> Project');
fs.mkdirSync('./dist/scaffold', { recursive: true } as any);
fs.writeFileSync('./dist/scaffold/project.json', JSON.stringify(project));