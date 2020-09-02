import { Workspace } from '@allors/domain/system';
import { extend as extendDomain } from '@allors/domain/custom';
import { extend as extendCore } from '@allors/gatsby/source/core';
import { extendMedia } from './domain/Media';
import { extendOrganisation } from './domain/Organisation';
import { extendPerson } from './domain/Person';

export function extend(workspace: Workspace) {
  extendDomain(workspace);
  extendCore(workspace);
  extendMedia(workspace);
  extendOrganisation(workspace);
  extendPerson(workspace);
}
