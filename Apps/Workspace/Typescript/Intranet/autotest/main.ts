#! /usr/bin/env node
import * as fs from 'fs';
import { data } from '../src/allors/meta';
import { MetaPopulation } from '../src/allors/framework';
import { menu } from '../src/app/main/main.menu';
import { appMeta } from '../src/app/app.meta';
import { create as createDialogs, edit as editDialogs } from '../src/app/app-dialogs.module';

console.log();
console.log('Autotest');
console.log('========');

fs.mkdirSync('./dist/autotest', { recursive: true } as any);

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
fs.writeFileSync('./dist/autotest/meta.json', JSON.stringify(meta));

console.log('- Menu');

fs.writeFileSync('./dist/autotest/menu.json', JSON.stringify(menu));

console.log('- Dialogs');

const create = Object.keys(createDialogs).map((v) => ({ id: v, component: createDialogs[v].name }));
const edit = Object.keys(editDialogs).map((v) => ({ id: v, component: editDialogs[v].name }));

const dialogs = {
  create,
  edit,
};

fs.writeFileSync('./dist/autotest/dialogs.json', JSON.stringify(dialogs));
