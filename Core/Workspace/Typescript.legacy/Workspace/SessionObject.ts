namespace Allors {
    export interface ISessionObject {
        id: string;
        version: string;
        objectType: Meta.ObjectType;

        isNew: boolean;

        session: ISession;
        workspaceObject: IWorkspaceObject;

        hasChanges: boolean;

        canRead(roleTypeName: string): boolean;
        canWrite(roleTypeName: string): boolean;
        canExecute(methodName: string): boolean;

        get(roleTypeName: string): any;
        set(roleTypeName: string, value: any) ;
        add(roleTypeName: string, value: any);
        remove(roleTypeName: string, value: any);

        save(): Data.PushRequestObject;
        saveNew(): Data.PushRequestNewObject;
        reset();
    }

    export interface INewSessionObject extends ISessionObject {
        newId: string;
    }

    export class SessionObject implements INewSessionObject {

        session : ISession;
        workspaceObject: IWorkspaceObject;
        objectType: Meta.ObjectType;

        newId: string;

        private changedRoleByRoleTypeName: { [id: string]: any; };
        private roleByRoleTypeName: { [id: string]: any; } = {};

        get isNew(): boolean {
            return this.newId ? true : false;
        }

        get hasChanges(): boolean {
            if (this.newId) {
                return true;
            }

            return this.changedRoleByRoleTypeName !== undefined;
        }
 
        get id(): string {
            return this.workspaceObject ? this.workspaceObject.id : this.newId;
        }

        get version(): string {
            return this.workspaceObject ? this.workspaceObject.version : undefined;
        }
        
        canRead(roleTypeName: string): boolean {
            if (this.newId) {
                return true;
            }
            else if (this.workspaceObject) {
                return this.workspaceObject.canRead(roleTypeName);
            } 

            return undefined;
        }

        canWrite(roleTypeName: string): boolean {
            if (this.newId) {
                return true;
            }
            else if (this.workspaceObject) {
                return this.workspaceObject.canWrite(roleTypeName);
            }

            return undefined;
        }

        canExecute(methodName: string): boolean {
            if (this.newId) {
                return true;
            }
            else if (this.workspaceObject) {
                return this.workspaceObject.canExecute(methodName);
            }

            return undefined;
        }

        get(roleTypeName: string): any {
            if (!this.roleByRoleTypeName) {
                return undefined;
            }

            let value = this.roleByRoleTypeName[roleTypeName];
            if (value === undefined) {
                const roleType = this.objectType.roleTypeByName[roleTypeName];
                if (this.newId === undefined) {
                    if (roleType.isUnit) {
                        value = this.workspaceObject.roles[roleTypeName];
                        if (value === undefined) {
                            value = null;
                        };
                    } else {
                        try {
                            if (roleType.isOne) {
                                const role: string = this.workspaceObject.roles[roleTypeName];
                                value = role ? this.session.get(role) : null;
                            } else {
                                const roles: string[] = this.workspaceObject.roles[roleTypeName];
                                value = roles ? roles.map(role => {
                                        return this.session.get(role);
                                }) : [];
                            }
                        }
                        catch (e) {
                            let value = "N/A";
                            try {
                                value = this.toString();
                            }
                            catch (e2) { };

                            throw new Error(`Could not get role ${roleTypeName} from [objectType: ${this.objectType.name}, id: ${this.id}, value: '${value}']`);
                        }
                    }
                } else {
                    if (roleType.isComposite && roleType.isMany) {
                        value = [];
                    } else {
                        value = null;
                    }                    
                }

                this.roleByRoleTypeName[roleTypeName] = value;
            }

            return value;
        }

        set(roleTypeName: string, value: any) {
            this.assertExists();

            if (this.changedRoleByRoleTypeName === undefined) {
                this.changedRoleByRoleTypeName = {};
            }

            if (value === undefined) {
                value = null;
            }

            if (value === null) {
                const roleType = this.objectType.roleTypeByName[roleTypeName];
                if (roleType.isComposite && roleType.isMany) {
                    value = [];
                }
            }

            this.roleByRoleTypeName[roleTypeName] = value;
            this.changedRoleByRoleTypeName[roleTypeName] = value;

            this.session.hasChanges = true;
        }

        add(roleTypeName: string, value: any) {
            this.assertExists();

            const roles = this.get(roleTypeName);
            if (roles.indexOf(value) < 0) {
                roles.push(value);
            }

            this.set(roleTypeName, roles);

            this.session.hasChanges = true;
        }

        remove(roleTypeName: string, value: any) {
            this.assertExists();

            const roles = this.get(roleTypeName);
            const index = roles.indexOf(value);
            if (index >= 0) {
                roles.splice(index, 1);
            }

            this.set(roleTypeName, roles);

            this.session.hasChanges = true;
        }

        save(): Data.PushRequestObject {
            if (this.changedRoleByRoleTypeName !== undefined) {
                const data = new Data.PushRequestObject();
                data.i = this.id;
                data.v = this.version;
                data.roles = this.saveRoles();
                return data;
            }

            return undefined;
        }

        saveNew(): Data.PushRequestNewObject {
            this.assertExists();

            const data = new Data.PushRequestNewObject();
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
            } else {
                this.workspaceObject = this.workspaceObject.workspace.get(this.id);
                this.roleByRoleTypeName = {};
            }

            delete this.changedRoleByRoleTypeName;
        }

        private assertExists() {
            if (!this.roleByRoleTypeName) {
                throw "Object doesn't exist anymore.";
            }
        }

        private saveRoles(): Data.PushRequestRole[] {
            var saveRoles = new Array<Data.PushRequestRole>();

            _.forEach(this.changedRoleByRoleTypeName, (role, roleTypeName) => {
                var roleType = this.objectType.roleTypeByName[roleTypeName];

                var saveRole = new Data.PushRequestRole;
                saveRole.t = roleType.name;

                if (roleType.isUnit) {
                    saveRole.s = role;
                } else {
                    if (roleType.isOne) {
                        saveRole.s = role ? role.id || role.newId : null;
                    } else {
                        const roleIds = role.map(item => { return (<SessionObject>item).id || (<SessionObject>item).newId; });
                        if (this.newId) {
                            saveRole.a = roleIds;
                        } else {
                            const originalRoleIds = <string[]>this.workspaceObject.roles[roleTypeName];
                            if (!originalRoleIds) {
                                saveRole.a = roleIds;
                            } else {
                                saveRole.a = _.difference(roleIds, originalRoleIds);
                                saveRole.r = _.difference(originalRoleIds, roleIds);
                            }
                        }
                    }
                }

                saveRoles.push(saveRole);
            });

            return saveRoles;
        }
    }
}