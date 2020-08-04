import { Injectable } from '@angular/core';
import { Workspace } from '@allors/domain/system';
import { WorkspaceConfig } from './WorkspaceConfig';

@Injectable()
export class WorkspaceService {
  public workspace: Workspace;

  constructor(config: WorkspaceConfig) {
    this.workspace = config.workspace;
  }
}
