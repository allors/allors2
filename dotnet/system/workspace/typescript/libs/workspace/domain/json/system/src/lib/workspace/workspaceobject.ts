import { Class, RelationType, RoleType } from '@allors/workspace/meta/system';
import { Database } from '../Database/Database';
import { Strategy } from '../Strategy';

export class WorkspaceObject {
  constructor(public database: Database, public identity: number, public cls: Class, public version: number, public roleByRelationType: Map<RelationType, any>) {}

  static fromOriginal(originalWorkspaceObject: WorkspaceObject, changedRoleByRoleType: Map<RelationType, unknown>): WorkspaceObject {
    const cls = originalWorkspaceObject.cls;

    const originalRoleByRelationType = originalWorkspaceObject.roleByRelationType;

    const roleByRelationType = new Map(
      Array.from(cls.roleTypes)
        .filter((roleType) => changedRoleByRoleType.has(roleType.relationType) || originalRoleByRelationType.has(roleType.relationType))
        .map((roleType) => {
          const relationType = roleType.relationType;
          if (changedRoleByRoleType.has(relationType)) {
            const role = changedRoleByRoleType.get(relationType);
            if (roleType.objectType.isUnit) {
              return [relationType, role];
            } else {
              if (roleType.isOne) {
                return [relationType, (role as Strategy).id];
              } else {
                const roles = role as Strategy[];
                return [relationType, roles.map((v) => v.id)];
              }
            }
          } else {
            const role = originalWorkspaceObject.roleByRelationType.get(relationType) as unknown;
            return [relationType, role];
          }
        })
    );

    return new WorkspaceObject(originalWorkspaceObject.database, originalWorkspaceObject.identity, cls, ++originalWorkspaceObject.version, roleByRelationType);
  }

  getRole(roleType: RoleType): unknown {
    return this.roleByRelationType?.get(roleType.relationType);
  }
}
