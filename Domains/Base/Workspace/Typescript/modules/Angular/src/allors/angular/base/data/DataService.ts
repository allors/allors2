import { Injectable } from '@angular/core';
import { WorkspaceService } from '../framework';
import { PullFactory, FetchFactory, TreeFactory, MetaDomain } from '../../../meta';

export const x = {};

@Injectable()
export class DataService {

  public readonly m: MetaDomain;

  public readonly pull: PullFactory;
  public readonly fetch: FetchFactory;
  public readonly tree: TreeFactory;

  constructor(private workspaceService: WorkspaceService) {
    const metaPopulation = workspaceService.metaPopulation;
    this.m = metaPopulation.metaDomain;
    this.pull = new PullFactory(metaPopulation);
    this.fetch = new FetchFactory(metaPopulation);
    this.tree = new TreeFactory(metaPopulation);
  }
}
