"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const PushRequestNewObject_1 = require("./../database/push/PushRequestNewObject");
const PushRequestObject_1 = require("./../database/push/PushRequestObject");
const PushRequestRole_1 = require("./../database/push/PushRequestRole");
class SessionObject {
    constructor() {
        this.roleByRoleTypeName = {};
    }
    get isNew() {
        return this.newId ? true : false;
    }
    get hasChanges() {
        if (this.newId) {
            return true;
        }
        return this.changedRoleByRoleTypeName !== undefined;
    }
    get id() {
        return this.workspaceObject ? this.workspaceObject.id : this.newId;
    }
    get version() {
        return this.workspaceObject ? this.workspaceObject.version : undefined;
    }
    canRead(roleTypeName) {
        if (this.newId) {
            return true;
        }
        else if (this.workspaceObject) {
            return this.workspaceObject.canRead(roleTypeName);
        }
        return undefined;
    }
    canWrite(roleTypeName) {
        if (this.newId) {
            return true;
        }
        else if (this.workspaceObject) {
            return this.workspaceObject.canWrite(roleTypeName);
        }
        return undefined;
    }
    canExecute(methodName) {
        if (this.newId) {
            return true;
        }
        else if (this.workspaceObject) {
            return this.workspaceObject.canExecute(methodName);
        }
        return undefined;
    }
    get(roleTypeName) {
        if (!this.roleByRoleTypeName) {
            return undefined;
        }
        let value = this.roleByRoleTypeName[roleTypeName];
        if (value === undefined) {
            const roleType = this.objectType.roleTypeByName[roleTypeName];
            if (this.newId === undefined) {
                if (roleType.objectType.isUnit) {
                    value = this.workspaceObject.roles[roleTypeName];
                    if (value === undefined) {
                        value = null;
                    }
                }
                else {
                    try {
                        if (roleType.isOne) {
                            const role = this.workspaceObject.roles[roleTypeName];
                            value = role ? this.session.get(role) : null;
                        }
                        else {
                            const roles = this.workspaceObject.roles[roleTypeName];
                            value = roles ? roles.map((role) => {
                                return this.session.get(role);
                            }) : [];
                        }
                    }
                    catch (e) {
                        let stringValue = "N/A";
                        try {
                            stringValue = this.toString();
                        }
                        catch (e2) {
                            throw new Error(`Could not get role ${roleTypeName} from [objectType: ${this.objectType.name}, id: ${this.id}]`);
                        }
                        throw new Error(`Could not get role ${roleTypeName} from [objectType: ${this.objectType.name}, id: ${this.id}, value: '${stringValue}']`);
                    }
                }
            }
            else {
                if (roleType.objectType.isComposite && roleType.isMany) {
                    value = [];
                }
                else {
                    value = null;
                }
            }
            this.roleByRoleTypeName[roleTypeName] = value;
        }
        return value;
    }
    set(roleTypeName, value) {
        this.assertExists();
        if (this.changedRoleByRoleTypeName === undefined) {
            this.changedRoleByRoleTypeName = {};
        }
        if (value === undefined) {
            value = null;
        }
        if (value === null) {
            const roleType = this.objectType.roleTypeByName[roleTypeName];
            if (roleType.objectType.isComposite && roleType.isMany) {
                value = [];
            }
        }
        this.roleByRoleTypeName[roleTypeName] = value;
        this.changedRoleByRoleTypeName[roleTypeName] = value;
        this.session.hasChanges = true;
    }
    add(roleTypeName, value) {
        this.assertExists();
        const roles = this.get(roleTypeName);
        if (roles.indexOf(value) < 0) {
            roles.push(value);
        }
        this.set(roleTypeName, roles);
        this.session.hasChanges = true;
    }
    remove(roleTypeName, value) {
        this.assertExists();
        const roles = this.get(roleTypeName);
        const index = roles.indexOf(value);
        if (index >= 0) {
            roles.splice(index, 1);
        }
        this.set(roleTypeName, roles);
        this.session.hasChanges = true;
    }
    save() {
        if (this.changedRoleByRoleTypeName !== undefined) {
            const data = new PushRequestObject_1.PushRequestObject();
            data.i = this.id;
            data.v = this.version;
            data.roles = this.saveRoles();
            return data;
        }
        return undefined;
    }
    saveNew() {
        this.assertExists();
        const data = new PushRequestNewObject_1.PushRequestNewObject();
        data.ni = this.newId;
        data.t = this.objectType.name;
        if (this.changedRoleByRoleTypeName !== undefined) {
            data.roles = this.saveRoles();
        }
        return data;
    }
    reset() {
        if (this.newId) {
            delete this.newId;
            delete this.session;
            delete this.objectType;
            delete this.roleByRoleTypeName;
        }
        else {
            this.workspaceObject = this.workspaceObject.workspace.get(this.id);
            this.roleByRoleTypeName = {};
        }
        delete this.changedRoleByRoleTypeName;
    }
    assertExists() {
        if (!this.roleByRoleTypeName) {
            throw new Error("Object doesn't exist anymore.");
        }
    }
    saveRoles() {
        const saveRoles = new Array();
        if (this.changedRoleByRoleTypeName) {
            Object
                .keys(this.changedRoleByRoleTypeName)
                .forEach((roleTypeName) => {
                const role = this.changedRoleByRoleTypeName[roleTypeName];
                const roleType = this.objectType.roleTypeByName[roleTypeName];
                const saveRole = new PushRequestRole_1.PushRequestRole();
                saveRole.t = roleType.name;
                if (roleType.objectType.isUnit) {
                    saveRole.s = role;
                }
                else {
                    if (roleType.isOne) {
                        saveRole.s = role ? role.id || role.newId : null;
                    }
                    else {
                        const roleIds = role.map((item) => item.id || item.newId);
                        if (this.newId) {
                            saveRole.a = roleIds;
                        }
                        else {
                            const originalRoleIds = this.workspaceObject.roles[roleTypeName];
                            if (!originalRoleIds) {
                                saveRole.a = roleIds;
                            }
                            else {
                                saveRole.a = roleIds.filter((v) => originalRoleIds.indexOf(v) < 0);
                                saveRole.r = originalRoleIds.filter((v) => roleIds.indexOf(v) < 0);
                            }
                        }
                    }
                }
                saveRoles.push(saveRole);
            });
        }
        return saveRoles;
    }
}
exports.SessionObject = SessionObject;
