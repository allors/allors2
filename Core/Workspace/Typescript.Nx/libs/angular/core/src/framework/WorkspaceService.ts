import { Injectable } from '@angular/core';
import { MetaPopulation } from '@allors/meta/system';
import { Workspace } from '@allors/domain/system';
import { data } from '@allors/meta/generated';
import { extend } from '@allors/domain/generated';

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
