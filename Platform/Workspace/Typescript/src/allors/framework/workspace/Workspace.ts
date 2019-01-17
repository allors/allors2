import { MetaPopulation } from '../meta';

import { PullResponse } from './../protocol/pull/PullResponse';
import { SyncRequest } from './../protocol/sync/SyncRequest';
import { SyncResponse } from './../protocol/sync/SyncResponse';

import { SessionObject } from './SessionObject';
import { WorkspaceObject } from './WorkspaceObject';

export interface IWorkspace {
    metaPopulation: MetaPopulation;
    constructorByName: { [name: string]: typeof SessionObject };
    diff(data: PullResponse): SyncRequest;
    sync(data: SyncResponse): void;
    get(id: string): WorkspaceObject;
}

export class Workspace implements IWorkspace {

    public constructorByName: { [name: string]: typeof SessionObject };

    public prototypeByName: { [name: string]: any };

    private workspaceObjectById: { [id: string]: WorkspaceObject } = {};

    workspaceObjectByIdByClassId: { [id: string]: { [id: string]: WorkspaceObject } } = {};

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

    public diff(response: PullResponse): SyncRequest {
        const requireLoadIds = new SyncRequest();

        if (response.objects) {
            const userSecurityHash = response.userSecurityHash;

            const requireLoadIdsWithVersion = response.objects.filter((idAndVersion) => {
                const [id, version] = idAndVersion;
                const workspaceObject = this.workspaceObjectById[id];

                return (workspaceObject === undefined) ||
                    (workspaceObject === null) ||
                    (workspaceObject.version !== version) ||
                    (workspaceObject.userSecurityHash !== userSecurityHash);
            });

            requireLoadIds.objects = requireLoadIdsWithVersion.map((idWithVersion) => {
                return idWithVersion[0];
            });
        }

        return requireLoadIds;
    }

    public sync(syncResponse: SyncResponse): void {
        if (syncResponse.objects) {
            Object
                .keys(syncResponse.objects)
                .forEach((v) => {
                    const objectData = syncResponse.objects[v];
                    const workspaceObject = new WorkspaceObject(this, syncResponse, objectData);
                    this.workspaceObjectById[workspaceObject.id] = workspaceObject;

                    this.addByObjectTypeId(workspaceObject);
                });
        }
    }

    public get(id: string): WorkspaceObject {
        const workspaceObject = this.workspaceObjectById[id];
        if (workspaceObject === undefined) {
            throw new Error(`Object with id ${id} is not present.`);
        }

        return workspaceObject;
    }

    private addByObjectTypeId(workspaceObject: WorkspaceObject){
        let workspaceObjectById = this.workspaceObjectByIdByClassId[workspaceObject.objectType.id];
        if (!workspaceObjectById) {
            workspaceObjectById = {};
            this.workspaceObjectByIdByClassId[workspaceObject.objectType.id] = workspaceObjectById;
        }

        workspaceObjectById[workspaceObject.id] = workspaceObject;
    }
}
