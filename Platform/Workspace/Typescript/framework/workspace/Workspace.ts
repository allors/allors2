import { SessionObject } from './SessionObject';
import { WorkspaceObject, IWorkspaceObject } from './WorkspaceObject';
import { AccessControl } from './AccessControl';
import { Permission } from './Permission';
import { Compressor } from '../protocol/Compressor';
import { MetaPopulation } from '../meta/MetaPopulation';
import { ObjectType } from '../meta/ObjectType';
import { PullResponse } from '../protocol/pull/PullResponse';
import { SyncRequest } from '../protocol/sync/SyncRequest';
import { SyncResponse } from '../protocol/sync/SyncResponse';
import { SecurityRequest } from '../protocol/security/SecurityRequest';
import { SecurityResponse } from '../protocol/security/SecurityResponse';
import { OperandType } from '../meta/OperandType';
import { Operations } from '../protocol/Operations';

export interface IWorkspace {
  metaPopulation: MetaPopulation;
  constructorByObjectType: Map<ObjectType, typeof SessionObject>;

  diff(data: PullResponse): SyncRequest;
  sync(data: SyncResponse): SecurityRequest | undefined;
  security(data: SecurityResponse): void;
  get(id: string): IWorkspaceObject | undefined;
  permission(objectType: ObjectType, operandType: OperandType, operation: Operations): Permission | undefined;
}

export class Workspace implements IWorkspace {
  constructorByObjectType: Map<ObjectType, any>;

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

    metaPopulation.classes.forEach((v) => {
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
      objectType.roleTypes.forEach((roleType) => {
        Object.defineProperty(prototype, 'CanRead' + roleType.name, {
          get(this: SessionObject) {
            return this.canRead(roleType);
          },
        });

        if (roleType.relationType.isDerived) {
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

      objectType.associationTypes.forEach((associationType) => {
        Object.defineProperty(prototype, associationType.name, {
          get(this: SessionObject) {
            return this.getAssociation(associationType);
          },
        });
      });

      objectType.methodTypes.forEach((methodType) => {
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

  get(id: string): IWorkspaceObject | undefined {
    const workspaceObject = this.workspaceObjectById.get(id);
    if (workspaceObject === undefined) {
      throw new Error(`Object with id ${id} is not present.`);
    }

    return workspaceObject ?? null;
  }

  getForAssociation(id: string): WorkspaceObject | undefined {
    return this.workspaceObjectById.get(id);
  }

  diff(response: PullResponse): SyncRequest {
    const syncRequest: SyncRequest = {
      objects: (response.objects ?? [])
        .filter((syncRequestObject) => {
          const [id, version, sortedAccessControlIds, sortedDeniedPermissionIds] = syncRequestObject;
          const workspaceObject = this.workspaceObjectById.get(id);

          const sync =
            workspaceObject == null ||
            workspaceObject.version !== version ||
            workspaceObject.sortedAccessControlIds !== sortedAccessControlIds ||
            workspaceObject.sortedDeniedPermissionIds !== sortedDeniedPermissionIds;

          return sync;
        })
        .map((syncRequestObject) => {
          return syncRequestObject[0];
        }),
    };

    return syncRequest;
  }

  sync(syncResponse: SyncResponse): SecurityRequest | undefined {
    const missingAccessControlIds = new Set<string>();
    const missingPermissionIds = new Set<string>();

    const sortedAccessControlIdsDecompress = (compressed: string): string => {
      if (compressed) {
        compressed.split(Compressor.itemSeparator).forEach((v) => {
          if (!this.accessControlById.has(v)) {
            missingAccessControlIds.add(v);
          }
        });
      }

      return compressed;
    };

    const sortedDeniedPermissionIdsDecompress = (compressed: string): string => {
      if (compressed) {
        compressed.split(Compressor.itemSeparator).forEach((v) => {
          if (!this.permissionById.has(v)) {
            missingPermissionIds.add(v);
          }
        });
      }

      return compressed;
    };

    if (syncResponse.objects) {
      syncResponse.objects.forEach((v) => {
        const workspaceObject = new WorkspaceObject(
          this,
          this.metaPopulation,
          v,
          sortedAccessControlIdsDecompress,
          sortedDeniedPermissionIdsDecompress,
        );

        this.add(workspaceObject);
      });
    }

    if (missingAccessControlIds.size > 0 || missingPermissionIds.size > 0) {
      const securityRequest: SecurityRequest = {
        accessControls: [...missingAccessControlIds],
        permissions: [...missingPermissionIds],
      };

      return securityRequest;
    }

    return undefined;
  }

  private getOrCreate<T, U, V>(map: Map<T, Map<U, V>>, key: T) {
    let value = map.get(key);
    if (!value) {
      value = new Map();
      map.set(key, value);
    }

    return value;
  }

  security(securityResponse: SecurityResponse): SecurityRequest | undefined {
    if (securityResponse.permissions) {
      securityResponse.permissions.forEach((v) => {
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

      securityResponse.accessControls.forEach((v) => {
        const id = v.i;
        const version = v.v;
        const permissionIds = new Set<string>();
        v.p?.split(',').forEach((w) => {
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
        const securityRequest: SecurityRequest = {
          permissions: [...missingPermissionIds],
        };

        return securityRequest;
      }
    }

    return undefined;
  }

  new(id: string, objectType: ObjectType): WorkspaceObject {
    const workspaceObject = new WorkspaceObject(this, objectType, id);
    this.add(workspaceObject);
    return workspaceObject;
  }

  permission(objectType: ObjectType, operandType: OperandType, operation: Operations): Permission | undefined {
    switch (operation) {
      case Operations.Read:
        return this.readPermissionByOperandTypeByClass.get(objectType)?.get(operandType);
      case Operations.Write:
        return this.writePermissionByOperandTypeByClass.get(objectType)?.get(operandType);
      default:
        return this.executePermissionByOperandTypeByClass.get(objectType)?.get(operandType);
    }
  }

  private add(workspaceObject: WorkspaceObject) {
    this.workspaceObjectById.set(workspaceObject.id, workspaceObject);
    this.workspaceObjectsByClass.get(workspaceObject.objectType)?.add(workspaceObject);
  }
}
