import { Injectable } from '@angular/core';

import { DatabaseService } from './DatabaseService';
import { WorkspaceService } from './WorkspaceService';
import { Context } from './Context';

@Injectable({
  providedIn: 'root',
})
export class ContextService {

  context: Context;

  constructor(
    databaseService: DatabaseService,
    workspaceService: WorkspaceService
  ) {

    const database = databaseService.database;
    const workspace = workspaceService.workspace;
    this.context = new Context(database, workspace);
  }
}
