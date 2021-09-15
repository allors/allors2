namespace Allors {
    export interface IWorkspaceObject {
        id: string;
        version: string;
        userSecurityHash: string;
        objectType: Meta.ObjectType;

        workspace: IWorkspace;
        roles: any;
        methods: any;

        canRead(roleTypeName: string): boolean;
        canWrite(roleTypeName: string): boolean;
        canExecute(methodName: string): boolean;
    }

    export class WorkspaceObject implements IWorkspaceObject {
        workspace: IWorkspace;
        roles: any;
        methods: any;

        private i: string;
        private v: string;
        private u: string;
        private t: string;

        constructor(workspace: IWorkspace, loadResponse: Data.SyncResponse, loadObject: Data.SyncResponseObject) {
            this.workspace = workspace;
            this.i = loadObject.i;
            this.v = loadObject.v;
            this.u = loadResponse.userSecurityHash;
            this.t = loadObject.t;

            this.roles = {};
            this.methods = {};

            var objectType = this.workspace.objectTypeById[this.t];

            _.forEach(loadObject.roles, role => {
                var [name, access] = role;
                var canRead = access.indexOf("r") !== -1;
                var canWrite = access.indexOf("w") !== -1;

                this.roles[`CanRead${name}`] = canRead;
                this.roles[`CanWrite${name}`] = canWrite;
                
                if (canRead) {
                    const roleType = objectType.roleTypeByName[name];
                    let value = role[2];
                    if (value && roleType.isUnit && roleType.objectType === "DateTime") {
                        value = new Date(value as string);
                    }
                    this.roles[name] = value;
                }

            });

            _.forEach(loadObject.methods, method => {
                var [name, access] = method;
                var canExecute = access.indexOf("x") !== -1;

                this.methods[`CanExecute${name}`] = canExecute;
            });
        }

        get id(): string {
            return this.i;
        }

        get version(): string {
            return this.v;
        }

        get userSecurityHash(): string {
            return this.u;
        }

        get objectType(): Meta.ObjectType {
            return this.workspace.objectTypeByName[this.t];
        }

        canRead(roleTypeName: string): boolean {
            return this.roles[`CanRead${roleTypeName}`];
        }

        canWrite(roleTypeName: string): boolean {
            return this.roles[`CanWrite${roleTypeName}`];
        }

        canExecute(methodName: string): boolean {
            return this.methods[`CanExecute${methodName}`];
        }
    }
}
