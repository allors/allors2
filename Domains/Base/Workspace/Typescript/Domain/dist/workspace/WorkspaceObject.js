"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class WorkspaceObject {
    constructor(workspace, loadResponse, loadObject) {
        this.workspace = workspace;
        this.i = loadObject.i;
        this.v = loadObject.v;
        this.u = loadResponse.userSecurityHash;
        this.t = loadObject.t;
        this.roles = {};
        this.methods = {};
        const objectType = this.workspace.metaPopulation.objectTypeByName[this.t];
        if (loadObject.roles) {
            loadObject.roles.forEach((role) => {
                const [name, access] = role;
                const canRead = access.indexOf("r") !== -1;
                const canWrite = access.indexOf("w") !== -1;
                this.roles[`CanRead${name}`] = canRead;
                this.roles[`CanWrite${name}`] = canWrite;
                if (canRead) {
                    const roleType = objectType.roleTypeByName[name];
                    let value = role[2];
                    if (value && roleType.objectType.isUnit && roleType.objectType.name === "DateTime") {
                        value = new Date(value);
                    }
                    this.roles[name] = value;
                }
            });
        }
        if (loadObject.methods) {
            loadObject.methods.forEach((method) => {
                const [name, access] = method;
                const canExecute = access.indexOf("x") !== -1;
                this.methods[`CanExecute${name}`] = canExecute;
            });
        }
    }
    get id() {
        return this.i;
    }
    get version() {
        return this.v;
    }
    get userSecurityHash() {
        return this.u;
    }
    get objectType() {
        return this.workspace.metaPopulation.objectTypeByName[this.t];
    }
    canRead(roleTypeName) {
        return this.roles[`CanRead${roleTypeName}`];
    }
    canWrite(roleTypeName) {
        return this.roles[`CanWrite${roleTypeName}`];
    }
    canExecute(methodName) {
        return this.methods[`CanExecute${methodName}`];
    }
}
exports.WorkspaceObject = WorkspaceObject;
