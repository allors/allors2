import * as fs from 'fs';
import { menu } from '@allors/angular/material/custom';

import '@allors/meta/core';
import '@allors/angular/core';
import { MetaPopulation } from '@allors/meta/system';
import { Workspace } from '@allors/domain/system';
import { data } from '@allors/meta/generated';

import { extend as extendDomain } from '@allors/domain/custom';
import { extend as extendAngular } from '@allors/angular/core';
import { configure as configureMaterial } from '@allors/angular/material/custom';

import { create, edit } from '../../../apps/intranet/src/app/app.module';

interface MetaInfo {
  id: string;
  list: string;
  overview: string;
}

describe('Scaffold', () => {
  it('meta and menu', () => {
    const metaPopulation = new MetaPopulation(data);
    const workspace = new Workspace(metaPopulation);

    // Extend
    extendDomain(workspace);
    extendAngular(workspace);
    
    // Configure
    configureMaterial(metaPopulation, null);

    const meta: MetaInfo[] = metaPopulation.composites.map((v) => {
      return {
        id: v.id,
        list: v.list,
        overview: v.overview,
      };
    });

    
    const dialogs = {
      create: Object.keys(create).map((v) => ({ id: v, component: create[v].name })),
      edit: Object.keys(edit).map((v) => ({ id: v, component: edit[v].name })),
    };

    fs.mkdirSync('./dist/scaffold', { recursive: true } as any);
    fs.writeFileSync('./dist/scaffold/meta.json', JSON.stringify(meta));
    fs.writeFileSync('./dist/scaffold/menu.json', JSON.stringify(menu));
    fs.writeFileSync('./dist/scaffold/dialogs.json', JSON.stringify(dialogs));
  });
});
