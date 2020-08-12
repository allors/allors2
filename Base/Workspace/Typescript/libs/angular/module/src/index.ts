import '@allors/angular/core';
import '@allors/angular/base';

import { Workspace } from '@allors/domain/system';
import { extend as extendBase } from '@allors/angular/core';
export function extend(workspace: Workspace) {
  extendBase(workspace);
}

export * from './module';
