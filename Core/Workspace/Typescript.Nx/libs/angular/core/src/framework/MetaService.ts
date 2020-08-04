import { Injectable } from '@angular/core';
import { Meta, PullFactory, FetchFactory, TreeFactory } from '@allors/meta/generated';
import { WorkspaceService } from './WorkspaceService';

@Injectable({
  providedIn: 'root',
})
export class MetaService {

  x = {};

  m: Meta;

  pull: PullFactory;
  fetch: FetchFactory;
  tree: TreeFactory;

  constructor(public workspaceService: WorkspaceService) {
    this.m = workspaceService.workspace.metaPopulation as Meta;
    this.pull = new PullFactory(this.m);
    this.fetch = new FetchFactory(this.m);
    this.tree = new TreeFactory(this.m);
  }
}
