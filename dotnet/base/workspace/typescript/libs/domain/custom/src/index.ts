import { Workspace } from '@allors/domain/system';

import { extend as extendBase } from "@allors/domain/base"
export function extend(workspace: Workspace) {
  extendBase(workspace);
}
