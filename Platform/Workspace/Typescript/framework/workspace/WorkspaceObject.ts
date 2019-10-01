import { ObjectType, PropertyType, MethodType } from '../meta';
import { SyncResponseObject } from '../protocol/sync/SyncResponseObject';
import { IWorkspace } from './Workspace';
import { Operations } from '../protocol/Operations';
import { Permission } from './Permission';

export interface IWorkspaceObject {
  workspace: IWorkspace;
  objectType: ObjectType;
  id: string;
  version: string;
  roles: { [roleTypeId: string]: any };
  isPermitted(permission: Permission): boolean;
}

export class WorkspaceObject implements IWorkspaceObject {
  workspace: IWorkspace;
  objectType: ObjectType;
  id: string;
  version: string;
  sortedAccessControlIds: string;
  sortedDenidPermissionIds: string;
  roles: { [roleTypeId: string]: any };

  constructor(workspace: IWorkspace, id: string, objectType: ObjectType);
  constructor(workspace: IWorkspace, syncResponseObject: SyncResponseObject);
  constructor(workspace: IWorkspace, syncResponseObjectOrId: SyncResponseObject | string) {
    this.workspace = workspace;
    this.roles = {};

    if (typeof syncResponseObjectOrId === 'string') {
      this.id = syncResponseObjectOrId;
      this.objectType = this.objectType;
    } else {
      const syncResponseObject = syncResponseObjectOrId;
      const metaObjectById = this.workspace.metaPopulation.metaObjectById;

      this.objectType = metaObjectById[syncResponseObject.t] as ObjectType;
      this.id = syncResponseObject.i;
      this.version = syncResponseObject.v;

      if (syncResponseObject.r) {
        syncResponseObject.r.forEach((role) => {
          this.roles[role.t] = role.v;
        });
      }
    }
  }

  isPermitted(permission: Permission): bool
  {
      if (permission == null)
      {
          return false;
      }

      if (this.accessControls == null && this.SortedAccessControlIds != null)
      {
          this.accessControls = this.SortedAccessControlIds.Split(Compressor.ItemSeparator).Select(v => this.Workspace.AccessControlById[long.Parse(v)]).ToArray();
          if (this.deniedPermissions != null)
          {
              this.deniedPermissions = this.SortedDeniedPermissionIds.Split(Compressor.ItemSeparator).Select(v => this.Workspace.PermissionById[long.Parse(v)]).ToArray();
          }
      }

      if (this.deniedPermissions != null && this.deniedPermissions.Contains(permission))
      {
          return false;
      }

      if (this.accessControls != null && this.accessControls.Length > 0)
      {
          return this.accessControls.Any(v => v.Permissions.Any(w => w == permission));
      }

      return false;
  }
}
