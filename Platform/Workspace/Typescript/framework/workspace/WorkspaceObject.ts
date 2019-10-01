import { ObjectType, MetaObject, RoleType } from '../meta';
import { SyncResponseObject } from '../protocol/sync/SyncResponseObject';
import { IWorkspace, Workspace } from './Workspace';
import { Permission } from './Permission';
import { AccessControl } from './AccessControl';
import { Compressor } from '../protocol/Compressor';
import { ids } from '../../meta';

export interface IWorkspaceObject {
  workspace: IWorkspace;
  objectType: ObjectType;
  id: string;
  version: string;
  roles: { [roleTypeId: string]: any };
  isPermitted(permission: Permission): boolean;
}

export class WorkspaceObject implements IWorkspaceObject {
  workspace: Workspace;
  objectType: ObjectType;
  id: string;
  version: string;
  sortedAccessControlIds: string;
  sortedDeniedPermissionIds: string;
  roles: Map<RoleType, any>;

  private accessControls: AccessControl[];

  private deniedPermissions: Permission[];

  constructor(workspace: Workspace, syncResponseObject: SyncResponseObject, sortedAccessControlIdsDecompress: (compressed: string) => string, sortedDeniedPermissionIdsDecompress: (compressed: string) => string, metaDecompress: (compressed: string) => MetaObject) {

    this.workspace = workspace;
    this.id = syncResponseObject.i;
    this.version = syncResponseObject.v;
    this.objectType = metaDecompress(syncResponseObject.t) as ObjectType;

    this.roles = new Map();
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
          }
        } else {
          if (roleType.isMany) {
            value = (value as string).split(Compressor.itemSeparator);
          }
        }

        this.roles.set(roleType, value);
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
        .map(v => this.workspace.accessControlById[v]);

      if (this.deniedPermissions != null) {
        this.deniedPermissions = this.sortedDeniedPermissionIds
          .split(Compressor.itemSeparator)
          .map(v => this.workspace.permissionById[v]);
      }
    }

    if (this.deniedPermissions && this.deniedPermissions.indexOf(permission) > -1) {
      return false;
    }

    if (this.accessControls) {
      return !!this.accessControls.filter(v => !!v.permissions.filter(w => w === permission));
    }

    return false;
  }

  invalidate() {
    this.version = '';
  }

}
