#! /usr/bin/env node
import * as fs from 'fs';
import * as path from 'path';
import { menu } from '../src/app/main/main.menu';

console.log();
console.log('Menu');
console.log('====');

fs.mkdirSync('./dist/tools', { recursive: true } as any);
fs.writeFileSync('./dist/tools/menu.json', JSON.stringify(menu));
