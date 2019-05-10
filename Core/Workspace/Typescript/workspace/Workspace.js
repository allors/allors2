"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const SyncRequest_1 = require("./../database/sync/SyncRequest");
const WorkspaceObject_1 = require("./WorkspaceObject");
class Workspace {
    constructor(metaPopulation, constructorByName) {
        this.metaPopulation = metaPopulation;
        this.constructorByName = constructorByName;
        this.workspaceObjectById = {};
    }
    diff(response) {
        const requireLoadIds = new SyncRequest_1.SyncRequest();
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
    sync(syncResponse) {
        if (syncResponse.objects) {
            Object
                .keys(syncResponse.objects)
                .forEach((v) => {
                const objectData = syncResponse.objects[v];
                const workspaceObject = new WorkspaceObject_1.WorkspaceObject(this, syncResponse, objectData);
                this.workspaceObjectById[workspaceObject.id] = workspaceObject;
            });
        }
    }
    get(id) {
        const workspaceObject = this.workspaceObjectById[id];
        if (workspaceObject === undefined) {
            throw new Error(`Object with id ${id} is not present.`);
        }
        return workspaceObject;
    }
}
exports.Workspace = Workspace;
