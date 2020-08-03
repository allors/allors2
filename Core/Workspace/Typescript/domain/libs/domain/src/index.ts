import { Workspace } from '@allors/workspace/domain';
import { extendPerson } from './custom/Person';

export * from './generated';

export function extend(workspace: Workspace) {
  extendPerson(workspace);
}
