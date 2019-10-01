import { MetaPopulation, ObjectType, OperandType } from '../meta';

import { PullResponse, } from './../protocol/pull/PullResponse';
import { SyncRequest } from './../protocol/sync/SyncRequest';
import { SyncResponse } from './../protocol/sync/SyncResponse';
import { SecurityRequest } from '../protocol/security/SecurityRequest';
import { SecurityResponse } from '../protocol/security/SecurityResponse';

import { Compressor } from '../protocol/Compressor';
import { Decompressor, createMetaDecompressor as createMetaDecompress } from '../protocol/Decompressor';
import { Operations } from '../protocol/Operations';

import { SessionObject } from './SessionObject';
import { WorkspaceObject } from './WorkspaceObject';
import { AccessControl } from './AccessControl';
import { Permission } from './Permission';

export interface IWorkspace {
  metaPopulation: MetaPopulation;
  constructorByName: { [name: string]: typeof SessionObject };
  diff(data: PullResponse): SyncRequest;
  sync(data: SyncResponse): SecurityRequest;
  security(data: SecurityResponse): void;
  invalidate(id: string, objectType: ObjectType);
  get(id: string): WorkspaceObject;
}

export class Workspace implements IWorkspace {
  constructorByName: { [name: string]: typeof SessionObject };
  prototypeByName: { [name: string]: any };
  workspaceObjectByIdByClassId: { [id: string]: { [id: string]: WorkspaceObject } } = {};
  private workspaceObjectById: { [id: string]: WorkspaceObject } = {};
  private accessControlById: { [id: string]: AccessControl } = {};
  private permissionById: { [id: string]: Permission } = {};
  private readPermissionByOperandTypeByClass: { [classId: string]: { [operandTypeId: string]: Permission } } = {};
  private writePermissionByOperandTypeByClass: { [classId: string]: { [operandTypeId: string]: Permission } } = {};
  private executePermissionByOperandTypeByClass: { [classId: string]: { [operandTypeId: string]: Permission } } = {};

  constructor(public metaPopulation: MetaPopulation) {

    this.prototypeByName = {};
    this.constructorByName = {};

    Object.keys(this.metaPopulation.objectTypeByName)
      .forEach(
        (objectTypeName) => {
          const objectType = this.metaPopulation.objectTypeByName[objectTypeName];
          if (objectType.isClass) {

            const DynamicClass = (() => {
              return function () {

                const prototype1 = Object.getPrototypeOf(this);
                const prototype2 = Object.getPrototypeOf(prototype1);

                prototype2.init.call(this);
              };
            })();

            DynamicClass.prototype = Object.create(SessionObject.prototype);
            DynamicClass.prototype.constructor = DynamicClass;
            const prototype = DynamicClass.prototype;

            this.prototypeByName[objectTypeName] = prototype;
            this.constructorByName[objectTypeName] = DynamicClass as any;

            Object.keys(objectType.roleTypeByName)
              .forEach((roleTypeName) => {
                const roleType = objectType.roleTypeByName[roleTypeName];

                Object.defineProperty(prototype, 'CanRead' + roleTypeName, {
                  get(this: SessionObject) {
                    return this.canRead(roleTypeName);
                  },
                });

                if (roleType.isDerived) {
                  Object.defineProperty(prototype, roleTypeName, {
                    get(this: SessionObject) {
                      return this.get(roleTypeName);
                    },
                  });
                } else {
                  Object.defineProperty(prototype, 'CanWrite' + roleTypeName, {
                    get(this: SessionObject) {
                      return this.canWrite(roleTypeName);
                    },
                  });

                  Object.defineProperty(prototype, roleTypeName, {
                    get(this: SessionObject) {
                      return this.get(roleTypeName);
                    },

                    set(this: SessionObject, value) {
                      this.set(roleTypeName, value);
                    },
                  });

                  if (roleType.isMany) {

                    prototype['Add' + roleType.singular] = function (this: SessionObject, value) {
                      return this.add(roleTypeName, value);
                    };

                    prototype['Remove' + roleType.singular] = function (this: SessionObject, value) {
                      return this.remove(roleTypeName, value);
                    };
                  }
                }
              });

            Object.keys(objectType.associationTypeByName)
              .forEach((associationTypeName) => {

                Object.defineProperty(prototype, associationTypeName, {
                  get(this: SessionObject) {
                    return this.getAssociation(associationTypeName);
                  },
                });
              });

            Object.keys(objectType.methodTypeByName)
              .forEach((methodTypeName) => {
                const methodType = objectType.methodTypeByName[methodTypeName];

                Object.defineProperty(prototype, 'CanExecute' + methodTypeName, {
                  get(this: SessionObject) {
                    return this.canExecute(methodTypeName);
                  },
                });

                Object.defineProperty(prototype, methodTypeName, {
                  get(this: SessionObject) {
                    return this.method(methodTypeName);
                  },
                });
              });
          }
        });
  }

  diff(response: PullResponse): SyncRequest {

    return new SyncRequest(
      {
        objects: response.objects
          .filter((syncRequestObject) => {
            const [id, version, soretedAccessControlIds, sortedDeniedPermissionIds] = syncRequestObject;
            const workspaceObject = this.workspaceObjectById[id];
            return (workspaceObject === undefined) ||
              (workspaceObject === null) ||
              (workspaceObject.version !== version) ||
              (workspaceObject.sortedAccessControlIds !== soretedAccessControlIds) ||
              (workspaceObject.sortedDenidPermissionIds !== sortedDeniedPermissionIds);
          })
          .map((syncRequestObject) => {
            return syncRequestObject[0];
          })
      }
    );
  }

  sync(syncResponse: SyncResponse): SecurityRequest {
    const decompressor = new Decompressor();
    const missingAccessControlIds = new Set<string>();
    const missingPermissionIds = new Set<string>();

    const sortedAccessControlIdsDecompressor = (compressed: string): string => {
      return decompressor.read(compressed, first => {
        first
          .split(Compressor.itemSeparator)
          .forEach(v => {
            if (Object.prototype.hasOwnProperty.call(this.accessControlById, v)) {
              missingAccessControlIds.add(v)
            }
          });
      });
    };

    const sortedDeniedPermissionIdsDecompressor = (compressed: string): string => {
      return decompressor.read(compressed, first => {
        first
          .split(Compressor.itemSeparator)
          .forEach(v => {
            if (Object.prototype.hasOwnProperty.call(this.permissionById, v)) {
              missingPermissionIds.add(v);
            }
          });
      });
    };

    if (syncResponse.objects) {
      syncResponse.objects
        .forEach((v) => {
          const workspaceObject = new WorkspaceObject(this, v);
          this.workspaceObjectById[workspaceObject.id] = workspaceObject;
          this.addByObjectTypeId(workspaceObject);
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

  security(securityResponse: SecurityResponse): void {
    const decompressor = new Decompressor();
    const metaDecompress = createMetaDecompress(decompressor, this.metaPopulation);

    const setPermission = (permissionByOperandTypeByClass: { [objectTypeId: string]: { [operandTypeId: string]: Permission } }, objectType: ObjectType, operandType: OperandType, permission: Permission) => {
      let permissionByOperandType = permissionByOperandTypeByClass[objectType.id];
      if (!permissionByOperandType) {
        permissionByOperandType = {};
        permissionByOperandTypeByClass[objectType.id] = permissionByOperandType;
      }

      permissionByOperandType[operandType.id] = permission;
    };

    if (securityResponse.permissions) {
      securityResponse.permissions.forEach(v => {
        const id = v[0];
        const objectType = metaDecompress(v[1]) as ObjectType;
        const operandType = metaDecompress(v[2]) as OperandType;
        const operation = Operations[v[3]];

        const permission = new Permission(id, objectType, operandType, operation);
        this.permissionById[id] = permission;

        switch (operation) {
          case Operations.Read:
            setPermission(this.readPermissionByOperandTypeByClass, objectType, operandType, permission);
            break;

          case Operations.Write:
            setPermission(this.readPermissionByOperandTypeByClass, objectType, operandType, permission);
            break;

          case Operations.Execute:
            setPermission(this.readPermissionByOperandTypeByClass, objectType, operandType, permission);
            break;
        }
      });
    }

    if (securityResponse.accessControls) {
      securityResponse.accessControls.forEach(v => {
        const id = v.i;
        const version = v.v;
        const permissions = v.p.map(w => this.permissionById[w]);

        const accessControl = new AccessControl(id, version, permissions);
        this.accessControlById[id] = accessControl;
      });
    }
  }

  invalidate(id: string, objectType: ObjectType){
    this.workspaceObjectById[id] = new WorkspaceObject(this, id, objectType)
  }

  get(id: string): WorkspaceObject {
    const workspaceObject = this.workspaceObjectById[id];
    if (workspaceObject === undefined) {
      throw new Error(`Object with id ${id} is not present.`);
    }

    return workspaceObject;
  }

  private addByObjectTypeId(workspaceObject: WorkspaceObject) {
    let workspaceObjectById = this.workspaceObjectByIdByClassId[workspaceObject.objectType.id];
    if (!workspaceObjectById) {
      workspaceObjectById = {};
      this.workspaceObjectByIdByClassId[workspaceObject.objectType.id] = workspaceObjectById;
    }

    workspaceObjectById[workspaceObject.id] = workspaceObject;
  }
}
