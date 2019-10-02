import { ObjectType, MetaObject, RoleType } from '../meta';
import { SyncResponseObject } from '../protocol/sync/SyncResponseObject';
import { IWorkspace, Workspace } from './Workspace';
import { Permission } from './Permission';
import { AccessControl } from './AccessControl';
import { Compressor } from '../protocol/Compressor';
import { ids } from '../../meta';

import Decimal from 'decimal.js';

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

  constructor(workspace: Workspace, syncResponseObject: SyncResponseObject, sortedAccessControlIdsDecompress: (compressed: string) => string, sortedDeniedPermissionIdsDecompress: (compressed: string) => string, metaDecompress: (compressed: string) => MetaObject) {

    this.workspace = workspace;
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
          switch (roleType.objectType.id) {
            case ids.Boolean:
              value = value === 'true';
              break;
            case ids.Float:
              value = parseFloat(value);
              break;
            case ids.Integer:
              value = parseInt(value, 10);
              break;
            case ids.Decimal:
              value = new Decimal(value);
              break;
          }
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
