#! /usr/bin/env node
import * as fs from 'fs';
import { project } from './config';

console.log();
console.log('Autotest');
console.log('========');

console.log('- Project');

fs.mkdirSync('./dist', { recursive: true } as any);
fs.writeFileSync('./dist/project.json', JSON.stringify(project));