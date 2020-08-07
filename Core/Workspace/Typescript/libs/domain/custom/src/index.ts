import { Workspace } from '@allors/domain/system';

import { extend as extendCore } from "@allors/domain/core"

import { extendPerson } from './Person';

export function extend(workspace: Workspace) {
  extendCore(workspace);
  
  extendPerson(workspace);
}
