import { Injectable } from '@angular/core';
import { WorkspaceService } from './WorkspaceService';
import { Scope } from './Scope';
import { MetaDomain, PullFactory, FetchFactory, TreeFactory } from '../../../meta';

@Injectable()
export class Allors {

  public readonly m: MetaDomain;

  public readonly pull: PullFactory;
  public readonly fetch: FetchFactory;
  public readonly tree: TreeFactory;

  public readonly scope: Scope;

  constructor(public workspaceService: WorkspaceService) {
    this.scope = workspaceService.createScope();

    const metaPopulation = workspaceService.metaPopulation;
    this.m = metaPopulation.metaDomain;
    this.pull = new PullFactory(metaPopulation);
    this.fetch = new FetchFactory(metaPopulation);
    this.tree = new TreeFactory(metaPopulation);
  }
}
