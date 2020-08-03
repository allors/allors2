import { Injectable } from '@angular/core';
import { MetaPopulation } from '@allors/workspace/meta';
import { Workspace } from '@allors/workspace/domain';
import { data } from '@allors/meta';
import { extend } from '@allors/domain';

@Injectable()
export class WorkspaceService {
  public metaPopulation: MetaPopulation;
  public workspace: Workspace;

  constructor() {
    this.metaPopulation = new MetaPopulation(data);
    this.workspace = new Workspace(this.metaPopulation);
    extend(this.workspace);
  }
}
