"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const PushRequest_1 = require("./../database/push/PushRequest");
const ResponseType_1 = require("./../database/ResponseType");
class Session {
    constructor(workspace) {
        this.workspace = workspace;
        this.sessionObjectById = {};
        this.newSessionObjectById = {};
        this.hasChanges = false;
    }
    get(id) {
        if (!id) {
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
    create(objectTypeName) {
        const constructor = this.workspace.constructorByName[objectTypeName];
        const newSessionObject = new constructor();
        newSessionObject.session = this;
        newSessionObject.objectType = this.workspace.metaPopulation.objectTypeByName[objectTypeName];
        newSessionObject.newId = (--Session.idCounter).toString();
        this.newSessionObjectById[newSessionObject.newId] = newSessionObject;
        this.hasChanges = true;
        return newSessionObject;
    }
    reset() {
        if (this.newSessionObjectById) {
            Object
                .keys(this.newSessionObjectById)
                .forEach((key) => this.newSessionObjectById[key].reset());
        }
        if (this.sessionObjectById) {
            Object
                .keys(this.sessionObjectById)
                .forEach((key) => this.sessionObjectById[key].reset());
        }
        this.hasChanges = false;
    }
    pushRequest() {
        const data = new PushRequest_1.PushRequest();
        data.newObjects = [];
        data.objects = [];
        if (this.newSessionObjectById) {
            Object
                .keys(this.newSessionObjectById)
                .forEach((key) => {
                const newSessionObject = this.newSessionObjectById[key];
                const objectData = newSessionObject.saveNew();
                if (objectData !== undefined) {
                    data.newObjects.push(objectData);
                }
            });
        }
        if (this.sessionObjectById) {
            Object
                .keys(this.sessionObjectById)
                .forEach((key) => {
                const sessionObject = this.sessionObjectById[key];
                const objectData = sessionObject.save();
                if (objectData !== undefined) {
                    data.objects.push(objectData);
                }
            });
        }
        return data;
    }
    pushResponse(pushResponse) {
        if (pushResponse.newObjects) {
            Object
                .keys(pushResponse.newObjects)
                .forEach((key) => {
                const pushResponseNewObject = pushResponse.newObjects[key];
                const newId = pushResponseNewObject.ni;
                const id = pushResponseNewObject.i;
                const newSessionObject = this.newSessionObjectById[newId];
                const syncResponse = {
                    hasErrors: false,
                    objects: [
                        {
                            i: id,
                            methods: [],
                            roles: [],
                            t: newSessionObject.objectType.name,
                            v: "",
                        },
                    ],
                    responseType: ResponseType_1.ResponseType.Sync,
                    userSecurityHash: "#",
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
            throw new Error("Not all new objects received ids");
        }
    }
}
Session.idCounter = 0;
exports.Session = Session;
//# sourceMappingURL=Session.js.map