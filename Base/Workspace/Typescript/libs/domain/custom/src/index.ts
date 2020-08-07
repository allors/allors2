import { Workspace } from '@allors/domain/system';

import { extend as extendCore } from "@allors/domain/core"
import { extend as extendBase } from "@allors/domain/base"

export function extend(workspace: Workspace) {
  extendCore(workspace);
  extendBase(workspace);
}
