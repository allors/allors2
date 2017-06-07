import { IWorkspace } from './Workspace';
import { ISessionObject, INewSessionObject } from './SessionObject';
import { PushRequest, PushResponse, SyncResponse , ResponseType } from './database';

export interface ISession {
    hasChanges: boolean;

    get(id: string): ISessionObject;

    create(objectTypeName: string): ISessionObject;

    pushRequest(): PushRequest;

    pushResponse(saveResponse: PushResponse): void;
    reset(): void;
}

export class Session implements ISession {
    private static idCounter = 0;

    hasChanges: boolean;

    private sessionObjectById: { [id: string]: ISessionObject; } = {};
    private newSessionObjectById: { [id: string]: INewSessionObject; } = {};

    constructor(private workspace: IWorkspace) {
        this.hasChanges = false;
    }

    get(id: string): ISessionObject {
        if (id === undefined || id === null) {
            return undefined;
        }

        let sessionObject = this.sessionObjectById[id];
        if (sessionObject === undefined) {
            sessionObject = this.newSessionObjectById[id];

            if (sessionObject === undefined) {
                const workspaceObject = this.workspace.get(id);

                const constructor = this.workspace.constructorByName[workspaceObject.objectType.name];
                sessionObject = new constructor();
                sessionObject.session = this;
                sessionObject.workspaceObject = workspaceObject;
                sessionObject.objectType = workspaceObject.objectType;

                this.sessionObjectById[sessionObject.id] = sessionObject;
            }
        }

        return sessionObject;
    }

    create(objectTypeName: string): ISessionObject {
        const constructor = this.workspace.constructorByName[objectTypeName];
        const newSessionObject: INewSessionObject = new constructor();
        newSessionObject.session = this;
        newSessionObject.objectType = this.workspace.metaPopulation.objectTypeByName[objectTypeName];
        newSessionObject.newId = (--Session.idCounter).toString();

        this.newSessionObjectById[newSessionObject.newId] = newSessionObject;

        this.hasChanges = true;

        return newSessionObject;
    }

    reset(): void {
        if (this.newSessionObjectById) {
            Object
                .keys(this.newSessionObjectById)
                .forEach((v) => this.newSessionObjectById[v].reset());
        }

        if (this.sessionObjectById) {
            Object
                .keys(this.sessionObjectById)
                .forEach((v) => this.sessionObjectById[v].reset());
        }

        this.hasChanges = false;
    }

    pushRequest(): PushRequest {
        const data = new PushRequest();
        data.newObjects = [];
        data.objects = [];

        if (this.newSessionObjectById) {
            Object
                .keys(this.newSessionObjectById)
                .forEach((v) => {
                    const newSessionObject = this.newSessionObjectById[v];
                    const objectData = newSessionObject.saveNew();
                    if (objectData !== undefined) {
                        data.newObjects.push(objectData);
                    }
                });
        }

        if (this.sessionObjectById) {
            Object
                .keys(this.sessionObjectById)
                .forEach((v) => {
                    const sessionObject = this.sessionObjectById[v];
                    const objectData = sessionObject.save();
                    if (objectData !== undefined) {
                        data.objects.push(objectData);
                    }
                });
        }

        return data;
    }

    pushResponse(pushResponse: PushResponse): void {
        if (pushResponse.newObjects) {
            Object
                .keys(pushResponse.newObjects)
                .forEach((v) => {
                    const pushResponseNewObject = pushResponse.newObjects[v];
                     const newId = pushResponseNewObject.ni;
                    const id = pushResponseNewObject.i;

                    const newSessionObject = this.newSessionObjectById[newId];

                    const syncResponse: SyncResponse = {
                        responseType: ResponseType.Sync,
                        userSecurityHash: '#', // This should trigger a load on next check
                        objects: [
                            {
                                i: id,
                                v: '',
                                t: newSessionObject.objectType.name,
                                roles: [],
                                methods: []
                            }
                        ]
                    };

                    delete (this.newSessionObjectById[newId]);
                    delete (newSessionObject.newId);

                    this.workspace.sync(syncResponse);
                    const workspaceObject = this.workspace.get(id);
                    newSessionObject.workspaceObject = workspaceObject;

                    this.sessionObjectById[id] = newSessionObject;
                });
        }

        if (Object.getOwnPropertyNames(this.newSessionObjectById).length !== 0) {
            throw new Error('Not all new objects received ids');
        }
    }
}
