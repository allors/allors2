/// <reference path="./Protocol/Sync/SyncRequest.ts" />
/// <reference path="./Protocol/Security/SecuriyRequest.ts" />
/// <reference path="./Protocol/Operations.ts" />
/// <reference path="./Protocol/Compressor.ts" />
/// <reference path="./Protocol/Decompressor.ts" />
namespace Allors {
    import MetaPopulation = Meta.MetaPopulation;
    import OperandType = Meta.OperandType;
    import ObjectType = Meta.ObjectType;

    import PullResponse = Protocol.PullResponse;
    import SyncRequest = Protocol.SyncRequest;
    import SyncResponse = Protocol.SyncResponse;
    import SecurityRequest = Protocol.SecurityRequest;
    import SecurityResponse = Protocol.SecurityResponse;
    import Operations = Protocol.Operations;
    import Decompressor = Protocol.Decompressor;
    import Compressor = Protocol.Compressor;
    import createMetaDecompressor = Protocol.createMetaDecompressor;

    export interface IWorkspace {
        metaPopulation: MetaPopulation;
        constructorByObjectType: Map<ObjectType, typeof SessionObject>;

        diff(data: PullResponse): SyncRequest;
        sync(data: SyncResponse): SecurityRequest;
        security(data: SecurityResponse): void;
        get(id: string): WorkspaceObject;
        permission(objectType: ObjectType, operandType: OperandType, operation: Operations): Permission;
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
                    return function () {
                        const prototype1 = Object.getPrototypeOf(this);
                        const prototype2 = Object.getPrototypeOf(prototype1);
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

                                prototype['Add' + roleType.singular] = function (this: SessionObject, value) {
                                    return this.add(roleType, value);
                                };

                                prototype['Remove' + roleType.singular] = function (this: SessionObject, value) {
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

        get(id: string): WorkspaceObject {
            const workspaceObject = this.workspaceObjectById.get(id);
            if (workspaceObject === undefined) {
                throw new Error(`Object with id ${id} is not present.`);
            }

            return workspaceObject;
        }

        getForAssociation(id: string): WorkspaceObject {
            const workspaceObject = this.workspaceObjectById.get(id);
            return workspaceObject;
        }

        diff(response: PullResponse): SyncRequest {

            const decompressor = new Decompressor();
            return new SyncRequest(
                {
                    objects: response.objects
                        .filter((syncRequestObject) => {
                            const [id, version, compressedSortedAccessControlIds, compressedSortedDeniedPermissionIds] = syncRequestObject;
                            const workspaceObject = this.workspaceObjectById.get(id);

                            const sortedAccessControlIds = decompressor.read(compressedSortedAccessControlIds, v => v);
                            const sortedDeniedPermissionIds = decompressor.read(compressedSortedDeniedPermissionIds, v => v);

                            return (workspaceObject === undefined) ||
                                (workspaceObject === null) ||
                                (workspaceObject.version !== version) ||
                                (workspaceObject.sortedAccessControlIds !== sortedAccessControlIds) ||
                                (workspaceObject.sortedDeniedPermissionIds !== sortedDeniedPermissionIds);
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
            const metaDecompress = createMetaDecompressor(decompressor, this.metaPopulation);

            const sortedAccessControlIdsDecompress = (compressed: string): string => {
                return decompressor.read(compressed, first => {
                    first
                        .split(Compressor.itemSeparator)
                        .forEach(v => {
                            if (!this.accessControlById.has(v)) {
                                missingAccessControlIds.add(v);
                            }
                        });
                });
            };

            const sortedDeniedPermissionIdsDecompress = (compressed: string): string => {
                return decompressor.read(compressed, first => {
                    first
                        .split(Compressor.itemSeparator)
                        .forEach(v => {
                            if (!this.permissionById.has(v)) {
                                missingPermissionIds.add(v);
                            }
                        });
                });
            };

            if (syncResponse.objects) {
                syncResponse.objects
                    .forEach((v) => {
                        const workspaceObject = new WorkspaceObject(this);
                        workspaceObject.sync(v, sortedAccessControlIdsDecompress, sortedDeniedPermissionIdsDecompress, metaDecompress);
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

        security(securityResponse: SecurityResponse): SecurityRequest {
            const decompressor = new Decompressor();
            const metaDecompress = createMetaDecompressor(decompressor, this.metaPopulation);

            if (securityResponse.permissions) {
                securityResponse.permissions.forEach(v => {
                    const id = v[0];
                    const objectType = metaDecompress(v[1]) as ObjectType;
                    const operandType = metaDecompress(v[2]) as OperandType;
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

            let missingPermissionIds: Set<string>;

            if (securityResponse.accessControls) {
                securityResponse.accessControls.forEach(v => {
                    const id = v.i;
                    const version = v.v;
                    const permissionIds = new Set<string>();
                    v.p.forEach(w => {
                        if (!this.permissionById.has(w)) {
                            if (!missingPermissionIds) {
                                missingPermissionIds = new Set();
                            }
                            missingPermissionIds.add(w);
                        }
                        permissionIds.add(w);
                    });
                    const accessControl = new AccessControl(id, version, permissionIds);
                    this.accessControlById.set(id, accessControl);
                });
            }

            if (missingPermissionIds) {
                return new SecurityRequest({
                    permissions: [...missingPermissionIds],
                });
            }
        }

        new(id: string, objectType: ObjectType): WorkspaceObject {
            const workspaceObject = new WorkspaceObject(this);
            workspaceObject.new(id, objectType);
            this.add(workspaceObject);
            return workspaceObject;
        }

        permission(objectType: ObjectType, operandType: OperandType, operation: Operations): Permission {
            switch (operation) {
                case Operations.Read:
                    return this.readPermissionByOperandTypeByClass.get(objectType).get(operandType);
                case Operations.Write:
                    return this.writePermissionByOperandTypeByClass.get(objectType).get(operandType);
                default:
                    return this.executePermissionByOperandTypeByClass.get(objectType).get(operandType);
            }
        }

        private add(workspaceObject: WorkspaceObject) {
            this.workspaceObjectById.set(workspaceObject.id, workspaceObject);
            this.workspaceObjectsByClass.get(workspaceObject.objectType).add(workspaceObject);
        }
    }

}
