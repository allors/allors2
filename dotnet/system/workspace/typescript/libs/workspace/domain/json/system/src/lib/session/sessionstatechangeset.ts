import { AssociationType, RoleType } from '@allors/workspace/meta/system';
import { Strategy } from '../Strategy';

export class SessionStateChangeSet {
  constructor(public roleByAssociationByRoleType: Map<RoleType, Map<Strategy, unknown>>, public associationByRoleByAssociationType: Map<AssociationType, Map<Strategy, unknown>>) {}
}
