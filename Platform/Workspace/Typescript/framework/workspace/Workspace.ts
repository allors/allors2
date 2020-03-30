import { MetaPopulation, ObjectType, OperandType } from '../meta';

import { PullResponse, } from './../protocol/pull/PullResponse';
import { SyncRequest } from './../protocol/sync/SyncRequest';
import { SyncResponse } from './../protocol/sync/SyncResponse';
import { SecurityRequest } from '../protocol/security/SecurityRequest';
import { SecurityResponse } from '../protocol/security/SecurityResponse';

import { Operations } from '../protocol/Operations';

import { SessionObject } from './SessionObject';
import { WorkspaceObject, IWorkspaceObject } from './WorkspaceObject';
import { AccessControl } from './AccessControl';
import { Permission } from './Permission';
import { Compressor } from '../protocol/Compressor';

export interface IWorkspace {
  metaPopulation: MetaPopulation;
  constructorByObjectType: Map<ObjectType, typeof SessionObject>;

  diff(data: PullResponse): SyncRequest;
  sync(data: SyncResponse): SecurityRequest | null;
  security(data: SecurityResponse): void;
  get(id: string): IWorkspaceObject | null;
  permission(objectType: ObjectType, operandType: OperandType, operation: Operations): Permission | null;
}

export class Workspace implements IWorkspace {

  constructorByObjectType: Map<ObjectType, typeof SessionObject>;

  workspaceObjectById: Map<string, WorkspaceObject>;
  workspaceObjectsByClass: Map<ObjectType, Set<WorkspaceObject>>;

  accessControlById: Map<string, AccessControl>;
  permissionById: Map<string, Permission>;

  private readPermissionByOperandTypeByClass: Map<ObjectType, Map<OperandType, Permission>>;
  private writePermissionByOperandTypeByClass: Map<ObjectType, Map<OperandType, Permission>>;
  private executePermissionByOperandTypeByClass: Map<ObjectType, Map<OperandType, Permission>>;

  constructor(public metaPopulation: MetaPopulation) {

    this.constructorByObjectType = new Map();

    this.workspaceObjectById = new Map();
    this.workspaceObjectsByClass = new Map();
    for (const objectType of metaPopulation.classes) {
      this.workspaceObjectsByClass.set(objectType, new Set());
    }

    this.accessControlById = new Map();
    this.permissionById = new Map();

    this.readPermissionByOperandTypeByClass = new Map();
    this.writePermissionByOperandTypeByClass = new Map();
    this.executePermissionByOperandTypeByClass = new Map();

    metaPopulation.classes.forEach(v => {
      this.readPermissionByOperandTypeByClass.set(v, new Map());
      this.writePermissionByOperandTypeByClass.set(v, new Map());
      this.executePermissionByOperandTypeByClass.set(v, new Map());
    });

    this.metaPopulation.classes.forEach((objectType) => {
      const DynamicClass = (() => {
        return function() {
          // @ts-ignore
          const prototype1 = Object.getPrototypeOf(this);
          const prototype2 = Object.getPrototypeOf(prototype1);
          // @ts-ignore
          prototype2.init.call(this);
        };
      })();

      DynamicClass.prototype = Object.create(SessionObject.prototype);
      DynamicClass.prototype.constructor = DynamicClass;
      this.constructorByObjectType.set(objectType, DynamicClass as any);

      const prototype = DynamicClass.prototype;
      objectType.roleTypes
        .forEach((roleType) => {
          Object.defineProperty(prototype, 'CanRead' + roleType.name, {
            get(this: SessionObject) {
              return this.canRead(roleType);
            },
          });

          if (roleType.isDerived) {
            Object.defineProperty(prototype, roleType.name, {
              get(this: SessionObject) {
                return this.get(roleType);
              },
            });
          } else {
            Object.defineProperty(prototype, 'CanWrite' + roleType.name, {
              get(this: SessionObject) {
                return this.canWrite(roleType);
              },
            });

            Object.defineProperty(prototype, roleType.name, {
              get(this: SessionObject) {
                return this.get(roleType);
              },

              set(this: SessionObject, value) {
                this.set(roleType, value);
              },
            });

            if (roleType.isMany) {

              prototype['Add' + roleType.singular] = function(this: SessionObject, value: SessionObject) {
                return this.add(roleType, value);
              };

              prototype['Remove' + roleType.singular] = function(this: SessionObject, value: SessionObject) {
                return this.remove(roleType, value);
              };
            }
          }
        });

      objectType.associationTypes
        .forEach((associationType) => {
          Object.defineProperty(prototype, associationType.name, {
            get(this: SessionObject) {
              return this.getAssociation(associationType);
            },
          });
        });

      objectType.methodTypes
        .forEach((methodType) => {
          Object.defineProperty(prototype, 'CanExecute' + methodType.name, {
            get(this: SessionObject) {
              return this.canExecute(methodType);
            },
          });

          Object.defineProperty(prototype, methodType.name, {
            get(this: SessionObject) {
              return this.method(methodType);
            },
          });
        });
    });
  }

  get(id: string): IWorkspaceObject | null {
    const workspaceObject = this.workspaceObjectById.get(id);
    if (workspaceObject === undefined) {
      throw new Error(`Object with id ${id} is not present.`);
    }

    return workspaceObject ?? null;
  }

  getForAssociation(id: string): WorkspaceObject | null {
    const workspaceObject = this.workspaceObjectById.get(id);
    return workspaceObject ?? null;
  }

  diff(response: PullResponse): SyncRequest {

    return new SyncRequest(
      {
        objects: (response.objects ?? [])
          .filter((syncRequestObject) => {
            const [id, version, sortedAccessControlIds, sortedDeniedPermissionIds] = syncRequestObject;
            const workspaceObject = this.workspaceObjectById.get(id);

            const sync = (workspaceObject == null) ||
              (workspaceObject.version !== version) ||
              (workspaceObject.sortedAccessControlIds !== (sortedAccessControlIds ?? null)) ||
              (workspaceObject.sortedDeniedPermissionIds !== (sortedDeniedPermissionIds ?? null));
            return sync;
          })
          .map((syncRequestObject) => {
            return syncRequestObject[0];
          })
      }
    );
  }

  sync(syncResponse: SyncResponse): SecurityRequest | null {
    const missingAccessControlIds = new Set<string>();
    const missingPermissionIds = new Set<string>();

    const sortedAccessControlIdsDecompress = (compressed: string): string => {
      if (compressed) {
        compressed
          .split(Compressor.itemSeparator)
          .forEach(v => {
            if (!this.accessControlById.has(v)) {
              missingAccessControlIds.add(v);
            }
          });
      }

      return compressed;
    };

    const sortedDeniedPermissionIdsDecompress = (compressed: string): string => {
      if (compressed) {
        compressed
          .split(Compressor.itemSeparator)
          .forEach(v => {
            if (!this.permissionById.has(v)) {
              missingPermissionIds.add(v);
            }
          });
      }

      return compressed;
    };

    if (syncResponse.objects) {
      syncResponse.objects
        .forEach((v) => {
          const workspaceObject = new WorkspaceObject(this);
          workspaceObject.sync(v, sortedAccessControlIdsDecompress, sortedDeniedPermissionIdsDecompress, this.metaPopulation);
          this.add(workspaceObject);
        });
    }

    if (missingAccessControlIds.size > 0 || missingPermissionIds.size > 0) {
      return new SecurityRequest({
        accessControls: [...missingAccessControlIds],
        permissions: [...missingPermissionIds],
      });
    }

    return null;
  }

  private getOrCreate<T, U, V>(map: Map<T, Map<U, V>>, key: T) {
    let value = map.get(key);
    if (!value) {
      value = new Map();
      map.set(key, value);
    }

    return value;
  }

  security(securityResponse: SecurityResponse): SecurityRequest | null {

    if (securityResponse.permissions) {
      securityResponse.permissions.forEach(v => {
        const id = v[0];
        const objectType = this.metaPopulation.metaObjectById.get(v[1]) as ObjectType;
        const operandType = this.metaPopulation.metaObjectById.get(v[2]) as OperandType;
        const operation = parseInt(v[3], 10);

        let permission = this.permissionById.get(id);
        if (!permission) {
          permission = new Permission(id, objectType, operandType, operation);
          this.permissionById.set(id, permission);
        }

        switch (operation) {
          case Operations.Read:
            this.getOrCreate(this.readPermissionByOperandTypeByClass, objectType).set(operandType, permission);
            break;

          case Operations.Write:
            this.getOrCreate(this.writePermissionByOperandTypeByClass, objectType).set(operandType, permission);
            break;

          case Operations.Execute:
            this.getOrCreate(this.executePermissionByOperandTypeByClass, objectType).set(operandType, permission);
            break;
        }
      });
    }

    if (securityResponse.accessControls) {
      let missingPermissionIds: Set<string> = new Set();

      securityResponse.accessControls.forEach(v => {
        const id = v.i;
        const version = v.v;
        const permissionIds = new Set<string>();
        v.p.forEach(w => {
          if (!this.permissionById.has(w)) {
            if (missingPermissionIds === undefined) {
              missingPermissionIds = new Set();
            }
            missingPermissionIds.add(w);
          }
          permissionIds.add(w);
        });
        const accessControl = new AccessControl(id, version, permissionIds);
        this.accessControlById.set(id, accessControl);
      });

      if (missingPermissionIds.size > 0) {
        return new SecurityRequest({
          permissions: [...missingPermissionIds],
        });
      }
    }

    return null;
  }

  new(id: string, objectType: ObjectType): WorkspaceObject {
    const workspaceObject = new WorkspaceObject(this);
    workspaceObject.new(id, objectType);
    this.add(workspaceObject);
    return workspaceObject;
  }

  permission(objectType: ObjectType, operandType: OperandType, operation: Operations): Permission | null {
    switch (operation) {
      case Operations.Read:
        return this.readPermissionByOperandTypeByClass.get(objectType)?.get(operandType) ?? null;
      case Operations.Write:
        return this.writePermissionByOperandTypeByClass.get(objectType)?.get(operandType) ?? null;
      default:
        return this.executePermissionByOperandTypeByClass.get(objectType)?.get(operandType) ?? null;
    }
  }

  private add(workspaceObject: WorkspaceObject) {
    this.workspaceObjectById.set(workspaceObject.id, workspaceObject);
    this.workspaceObjectsByClass.get(workspaceObject.objectType)?.add(workspaceObject);
  }
}
