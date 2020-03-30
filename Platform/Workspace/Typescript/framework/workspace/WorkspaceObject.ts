import { ObjectType, MetaObject, RoleType, MetaPopulation } from '../meta';
import { SyncResponseObject } from '../protocol/sync/SyncResponseObject';
import { IWorkspace, Workspace } from './Workspace';
import { Permission } from './Permission';
import { AccessControl } from './AccessControl';
import { Compressor } from '../protocol/Compressor';
import { deserialize } from './SessionObject';

export interface IWorkspaceObject {
  workspace: IWorkspace;
  objectType: ObjectType;
  id: string;
  version: string;
  roleByRoleTypeId: Map<string, any>;

  isPermitted(permission: Permission): boolean;
}

export class WorkspaceObject implements IWorkspaceObject {
  workspace: Workspace & IWorkspace;
  objectType: ObjectType;
  id: string;
  version: string;
  sortedAccessControlIds: string | null;
  sortedDeniedPermissionIds: string | null;

  roleByRoleTypeId: Map<string, any>;

  private cachedSortedAccessControlIds: string | null;
  private cachedSortedDeniedPermissionIds: string | null;
  private cachedAccessControls: (AccessControl | undefined)[] | null;
  private cachedDeniedPermissions: Set<Permission | null> | null;

  constructor(workspace: Workspace) {
    this.workspace = workspace;
    this.cachedAccessControls = null;
    this.cachedDeniedPermissions = null;
    this.roleByRoleTypeId = new Map();
  }

  new(id: string, objectType: ObjectType) {
    this.objectType = objectType;
    this.id = id;
    this.version = '0';
  }

  sync(
    syncResponseObject: SyncResponseObject,
    sortedAccessControlIdsDecompress: (compressed: string) => string,
    sortedDeniedPermissionIdsDecompress: (compressed: string) => string,
    metaPopulation: MetaPopulation) {

    this.id = syncResponseObject.i;
    this.version = syncResponseObject.v;
    this.objectType = metaPopulation.metaObjectById.get(syncResponseObject.t) as ObjectType;

    this.roleByRoleTypeId = new Map();
    if (syncResponseObject.r) {
      syncResponseObject.r.forEach((role) => {
        const roleTypeId = role.t;
        const roleType = metaPopulation.metaObjectById.get(roleTypeId) as RoleType;

        let value: any = role.v;
        if (roleType.objectType.isUnit) {
          value = deserialize(value, roleType.objectType);
        } else {
          if (roleType.isMany) {
            value = (value as string).split(Compressor.itemSeparator);
          }
        }

        this.roleByRoleTypeId.set(roleType.id, value);
      });
    }

    this.sortedAccessControlIds = syncResponseObject?.a ? sortedAccessControlIdsDecompress(syncResponseObject.a) : null;
    this.sortedDeniedPermissionIds = syncResponseObject?.d ? sortedDeniedPermissionIdsDecompress(syncResponseObject.d) : null;
  }

  isPermitted(permission: Permission): boolean {
    if (permission == null) {
      return false;
    }

    if (this.sortedAccessControlIds !== this.cachedSortedAccessControlIds) {
      this.cachedSortedAccessControlIds = this.sortedAccessControlIds;
      if (this.sortedAccessControlIds) {
        this.cachedAccessControls = this.sortedAccessControlIds
          .split(Compressor.itemSeparator)
          .map(v => this.workspace.accessControlById.get(v));
      } else {
        this.sortedAccessControlIds = null;
      }
    }

    if (this.sortedDeniedPermissionIds !== this.cachedSortedDeniedPermissionIds) {
      this.cachedSortedDeniedPermissionIds = this.sortedDeniedPermissionIds;
      if (this.sortedDeniedPermissionIds) {
        this.cachedDeniedPermissions = new Set();
        this.sortedDeniedPermissionIds
          .split(Compressor.itemSeparator)
          // @ts-ignore
          .forEach(v => this.cachedDeniedPermissions.add(this.workspace.permissionById.get(v)));
      } else {
        this.cachedDeniedPermissions = null;
      }
    }

    if (this.cachedDeniedPermissions && this.cachedDeniedPermissions.has(permission)) {
      return false;
    }

    if (this.cachedAccessControls) {
      for (const accessControl of this.cachedAccessControls) {
        if (accessControl?.permissionIds.has(permission.id)) {
          return true;
        }
      }
    }

    return false;
  }

  invalidate() {
    this.version = '';
  }

}
