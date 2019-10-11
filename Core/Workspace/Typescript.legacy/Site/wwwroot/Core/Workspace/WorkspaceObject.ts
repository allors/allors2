namespace Allors {
  import MetaObject = Meta.MetaObject;
  import ObjectType = Meta.ObjectType;
  import RoleType = Meta.RoleType;

  import SyncResponseObject = Protocol.SyncResponseObject;
  import Compressor = Protocol.Compressor;
  import deserialize = Protocol.deserialize;

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

        private cachedSortedAccessControlIds: string;
        private cachedSortedDeniedPermissionIds: string;
        private cachedAccessControls: AccessControl[];
        private cachedDeniedPermissions: Set<Permission>;

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
                    if (accessControl.permissionIds.has(permission.id)) {
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

}
