namespace Allors {
    export interface ISession {
        hasChanges: boolean;

        get(id: string): ISessionObject;

        create(objectTypeName: string): ISessionObject;

        pushRequest(): Data.PushRequest;
        pushResponse(saveResponse: Data.PushResponse): void;
        reset(): void;
    }

    export class Session implements ISession {
        hasChanges: boolean;

        private static idCounter = 0;

        private workspace: IWorkspace;
        private sessionObjectById: { [id: string]: ISessionObject; } = {};
        private newSessionObjectById: { [id: string]: INewSessionObject; } = {};

        constructor(workspace: IWorkspace) {
            this.workspace = workspace;
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

                    const type = Domain[workspaceObject.objectType.name];
                    sessionObject = new type();
                    sessionObject.session = this;
                    sessionObject.workspaceObject = workspaceObject;
                    sessionObject.objectType = workspaceObject.objectType;

                    this.sessionObjectById[sessionObject.id] = sessionObject;
                }
            }

            return sessionObject;
        }

        create(objectTypeName: string): ISessionObject {
            const type = Domain[objectTypeName];
            const newSessionObject: INewSessionObject = new type();
            newSessionObject.session = this;
            newSessionObject.objectType = this.workspace.objectTypeByName[objectTypeName];
            newSessionObject.newId = (--Session.idCounter).toString();

            this.newSessionObjectById[newSessionObject.newId] = newSessionObject;

            this.hasChanges = true;

            return newSessionObject;
        }

        reset(): void {
            _.forEach(this.newSessionObjectById, v => {
                v.reset();
            });

            this.newSessionObjectById = {};

            _.forEach(this.sessionObjectById, v => {
                v.reset();
            });

            this.hasChanges = false;
        }

        pushRequest(): Data.PushRequest {
            var data = new Data.PushRequest();
            data.newObjects = [];
            data.objects = [];

            if (this.newSessionObjectById) {
                _.forEach(this.newSessionObjectById, newSessionObject => {
                    var objectData = newSessionObject.saveNew();
                    if (objectData !== undefined) {
                        data.newObjects.push(objectData);
                    }
                });
            }

            _.forEach(this.sessionObjectById, sessionObject => {
                var objectData = sessionObject.save();
                if (objectData !== undefined) {
                    data.objects.push(objectData);
                }
            });

            return data;
        }
        
        pushResponse(pushResponse: Data.PushResponse): void {
            if (pushResponse.newObjects) {
                _.forEach(pushResponse.newObjects, pushResponseNewObject => {
                    var newId = pushResponseNewObject.ni;
                    var id = pushResponseNewObject.i;

                    var newSessionObject = this.newSessionObjectById[newId];

                    var syncResponse: Data.SyncResponse = {
                        responseType: Data.ResponseType.Sync,
                        userSecurityHash: "#", // This should trigger a load on next check
                        objects: [
                            {
                                i: id,
                                v: "",
                                t: newSessionObject.objectType.name,
                                roles: [],
                                methods: []
                            }
                        ]
                    }

                    delete (this.newSessionObjectById[newId]);
                    delete(newSessionObject.newId);

                    this.workspace.sync(syncResponse);
                    var workspaceObject = this.workspace.get(id);
                    newSessionObject.workspaceObject = workspaceObject;

                    this.sessionObjectById[id] = newSessionObject;
                });
            }

            if (Object.getOwnPropertyNames(this.newSessionObjectById).length !== 0) {
                throw new Error("Not all new objects received ids");
            }
        }
    }
}