#! /usr/bin/env node
import * as fs from 'fs';
import { menu } from '../src/app/main/main.menu';

console.log();
console.log('Autotest');
console.log('========');

console.log('- Menu');
fs.mkdirSync('./dist/autotest', { recursive: true } as any);
fs.writeFileSync('./dist/autotest/menu.json', JSON.stringify(menu));
