#! /usr/bin/env node
import * as fs from 'fs';
import { data } from '../src/allors/meta';
import { MetaPopulation } from '../src/allors/framework';
import { menu } from '../src/app/main/main.menu';
import { appMeta } from '../src/app/app.meta';

console.log();
console.log('Autotest');
console.log('========');

console.log('- Meta');

interface MetaInfo {
  id: string;
  list: string;
  overview: string;
}

const metaPopulation = new MetaPopulation(data);
appMeta(metaPopulation);
const meta: MetaInfo[] = metaPopulation.composites.map((v) => {
  return {
    id: v.id,
    list: v.list,
    overview: v.overview,
  };
});
fs.mkdirSync('./dist/autotest', { recursive: true } as any);
fs.writeFileSync('./dist/autotest/meta.json', JSON.stringify(meta));


console.log('- Menu');

fs.mkdirSync('./dist/autotest', { recursive: true } as any);
fs.writeFileSync('./dist/autotest/menu.json', JSON.stringify(menu));
