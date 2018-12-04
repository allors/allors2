import { Injectable } from '@angular/core';

import { MetaDomain, PullFactory, FetchFactory, TreeFactory } from '../../../meta';
import { WorkspaceService } from './WorkspaceService';

@Injectable({
  providedIn: 'root',
})
export class MetaService {

  x = {};

  m: MetaDomain;

  pull: PullFactory;
  fetch: FetchFactory;
  tree: TreeFactory;

  constructor(public workspaceService: WorkspaceService) {
    const metaPopulation = workspaceService.metaPopulation;
    this.m = metaPopulation.metaDomain;
    this.pull = new PullFactory(metaPopulation);
    this.fetch = new FetchFactory(metaPopulation);
    this.tree = new TreeFactory(metaPopulation);
  }
}
