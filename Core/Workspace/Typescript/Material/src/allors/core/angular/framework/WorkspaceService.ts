import { Injectable } from '@angular/core';
import { MetaPopulation, Workspace } from '@allors/framework';
import { data } from '@allors/meta';
import { domain } from '@allors/domain';

@Injectable()
export class WorkspaceService {
  public metaPopulation: MetaPopulation;
  public workspace: Workspace;

  constructor() {
    this.metaPopulation = new MetaPopulation(data);
    this.workspace = new Workspace(this.metaPopulation);
    domain.apply(this.workspace);
  }
}
