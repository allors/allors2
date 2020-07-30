import { Injectable } from '@angular/core';
import { domain } from '@allors/domain';
import { MetaPopulation, Workspace } from '@allors/framework';
import { data } from '@allors/meta';

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
