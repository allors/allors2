import { SyncResponseObject, SyncResponseRole } from '@allors/protocol/json/system';
import { Class, RelationType, RoleType } from '@allors/workspace/meta/system';
import { fromString } from '@allors/workspace/domain/system';
import { Numbers } from '../collections/Numbers';
import { Database } from './Database';
import { AccessControl } from './Security/AccessControl';
import { Permission } from './Security/Permission';
import { ResponseContext } from './Security/ResponseContext';

export class DatabaseObject {
  version: number;
  accessControlIds: Numbers;
  deniedPermissionIds: Numbers;

  private _roleByRelationType?: Map<RelationType, unknown>;
  private _syncResponseRoles?: SyncResponseRole[];
  private _accessControls?: AccessControl[];
  private _deniedPermissions?: Permission[];

  constructor(public readonly database: Database, public readonly identity: number, public readonly cls: Class) {
    this.version = 0;
  }

  static fromResponse(database: Database, ctx: ResponseContext, syncResponseObject: SyncResponseObject): DatabaseObject {
    const obj = new DatabaseObject(database, syncResponseObject.i, database.metaPopulation.metaObjectByTag.get(syncResponseObject.t) as Class);
    obj.version = syncResponseObject.v;
    obj._syncResponseRoles = syncResponseObject.r;
    obj.accessControlIds = ctx.checkForMissingAccessControls(syncResponseObject.a);
    obj.deniedPermissionIds = ctx.checkForMissingPermissions(syncResponseObject.d);
    return obj;
  }

  get roleByRelationType(): Map<RelationType, unknown> {
    if (this._syncResponseRoles != null) {
      const meta = this.database.metaPopulation;
      this._roleByRelationType = new Map(
        this._syncResponseRoles.map((v) => {
          const relationType = meta.metaObjectByTag.get(v.t) as RelationType;
          const roleType = relationType.roleType;
          const objectType = roleType.objectType;
          let role: unknown;

          if (objectType.isUnit) {
            role = fromString(objectType.tag, v.v);
          } else {
            if (roleType.isOne) {
              role = v.o;
            } else {
              role = v.c;
            }
          }

          return [relationType, role];
        })
      );

      delete this._syncResponseRoles;
    }

    return this._roleByRelationType ?? new Map();
  }

  get accessControls(): AccessControl[] {
    return (this._accessControls ??= this.accessControlIds?.map((v) => this.database.accessControlById.get(v) as AccessControl) ?? []);
  }

  get deniedPermissions(): Permission[] {
    return (this._deniedPermissions ??= this.deniedPermissionIds?.map((v) => this.database.permissionById.get(v) as Permission) ?? []);
  }

  getRole(roleType: RoleType): unknown {
    return this.roleByRelationType.get(roleType.relationType);
  }

  isPermitted(permission: Permission): boolean {
    return !!permission && !this.deniedPermissions.includes(permission) && !!this.accessControls.filter((v) => v.permissionIds.has(permission.id)).length;
  }

  updateAccessControlIds(updatedAccessControlIds: number[]) {
    this.accessControlIds = updatedAccessControlIds;
    delete(this._accessControls);
  }

  updateDeniedPermissionIds(updatedDeniedPermissionIds: number[]) {
    this.deniedPermissionIds = updatedDeniedPermissionIds;
    delete(this._deniedPermissions);
  }
}
