import { ObjectType, MetaObject, RoleType } from '../meta';
import { SyncResponseObject } from '../protocol/sync/SyncResponseObject';
import { IWorkspace, Workspace } from './Workspace';
import { Permission } from './Permission';
import { AccessControl } from './AccessControl';
import { Compressor } from '../protocol/Compressor';
import { deserialize } from '../protocol/Serialization';

export interface IWorkspaceObject {
  workspace: IWorkspace;
  objectType: ObjectType;
  id: string;
  version: string;
  roleByRoleTypeId: Map<string, any>;

  isPermitted(permission: Permission): boolean;
}

export class WorkspaceObject implements IWorkspaceObject {
  workspace: Workspace;
  objectType: ObjectType;
  id: string;
  version: string;
  sortedAccessControlIds: string;
  sortedDeniedPermissionIds: string;

  roleByRoleTypeId: Map<string, any>;

  private accessControls: AccessControl[];
  private deniedPermissions: Permission[];

  constructor(workspace: Workspace) {
    this.workspace = workspace;
  }

  new(id: string, objectType: ObjectType) {
    this.objectType = objectType;
    this.id = id;
    this.version = '0';
  }

  sync(syncResponseObject: SyncResponseObject,
    sortedAccessControlIdsDecompress: (compressed: string) => string,
    sortedDeniedPermissionIdsDecompress: (compressed: string) => string,
    metaDecompress: (compressed: string) => MetaObject) {

    this.id = syncResponseObject.i;
    this.version = syncResponseObject.v;
    this.objectType = metaDecompress(syncResponseObject.t) as ObjectType;

    this.roleByRoleTypeId = new Map();
    if (syncResponseObject.r) {
      syncResponseObject.r.forEach((role) => {
        const roleTypeId = role.t;
        const roleType = metaDecompress(roleTypeId) as RoleType;

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

    this.sortedAccessControlIds = sortedAccessControlIdsDecompress(syncResponseObject.a);
    this.sortedDeniedPermissionIds = sortedDeniedPermissionIdsDecompress(syncResponseObject.d);
  }

  isPermitted(permission: Permission): boolean {
    if (permission == null) {
      return false;
    }

    if (!this.accessControls && this.sortedAccessControlIds) {
      this.accessControls = this.sortedAccessControlIds
        .split(Compressor.itemSeparator)
        .map(v => this.workspace.accessControlById.get(v));

      if (this.deniedPermissions != null) {
        this.deniedPermissions = this.sortedDeniedPermissionIds
          .split(Compressor.itemSeparator)
          .map(v => this.workspace.permissionById.get(v));
      }
    }

    if (this.deniedPermissions && this.deniedPermissions.indexOf(permission) > -1) {
      return false;
    }

    if (this.accessControls) {
      for (const accessControl of this.accessControls) {
        for (const effectivePermission of accessControl.permissions) {
          if (effectivePermission === permission) {
            return true;
          }
        }
      }
    }

    return false;
  }

  invalidate() {
    this.version = '';
  }

}
