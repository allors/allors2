﻿import { IWorkspace } from "./Workspace";
import { ISessionObject, INewSessionObject } from "./SessionObject";
import { PushRequest } from "./data/requests/PushRequest";
import { PushResponse } from "./data/responses/PushResponse";
import { SyncResponse } from "./data/responses/SyncResponse";
import { ResponseType } from "./data/responses/ResponseType";

import * as _ from "lodash";

export interface ISession {
    hasChanges: boolean;

    get(id: string): ISessionObject;

    create(objectTypeName: string): ISessionObject;

    pushRequest(): PushRequest;

    pushResponse(saveResponse: PushResponse): void;
    reset(): void;
}

export class Session implements ISession {
    hasChanges: boolean;

    private static idCounter = 0;

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
        _.forEach(this.newSessionObjectById, v => {
            v.reset();
        });

        this.newSessionObjectById = {};

        _.forEach(this.sessionObjectById, v => {
            v.reset();
        });

        this.hasChanges = false;
    }

    pushRequest(): PushRequest {
        let data = new PushRequest();
        data.newObjects = [];
        data.objects = [];

        if (this.newSessionObjectById) {
            _.forEach(this.newSessionObjectById, newSessionObject => {
                let objectData = newSessionObject.saveNew();
                if (objectData !== undefined) {
                    data.newObjects.push(objectData);
                }
            });
        }

        _.forEach(this.sessionObjectById, sessionObject => {
            let objectData = sessionObject.save();
            if (objectData !== undefined) {
                data.objects.push(objectData);
            }
        });

        return data;
    }

    pushResponse(pushResponse: PushResponse): void {
        if (pushResponse.newObjects) {
            _.forEach(pushResponse.newObjects, pushResponseNewObject => {
                let newId = pushResponseNewObject.ni;
                let id = pushResponseNewObject.i;

                let newSessionObject = this.newSessionObjectById[newId];

                let syncResponse: SyncResponse = {
                    responseType: ResponseType.Sync,
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
                };

                delete (this.newSessionObjectById[newId]);
                delete(newSessionObject.newId);

                this.workspace.sync(syncResponse);
                let workspaceObject = this.workspace.get(id);
                newSessionObject.workspaceObject = workspaceObject;

                this.sessionObjectById[id] = newSessionObject;
            });
        }

        if (Object.getOwnPropertyNames(this.newSessionObjectById).length !== 0) {
            throw new Error("Not all new objects received ids");
        }
    }
}
