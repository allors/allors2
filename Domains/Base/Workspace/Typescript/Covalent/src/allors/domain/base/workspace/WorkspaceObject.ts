import { SyncResponse, SyncResponseObject } from '../database';
import { ObjectType } from '../../../meta';

import { IWorkspace } from './Workspace';

export interface IWorkspaceObject {
    id: string;
    version: string;
    userSecurityHash: string;
    objectType: ObjectType;

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

    constructor(workspace: IWorkspace, loadResponse: SyncResponse, loadObject: SyncResponseObject) {
        this.workspace = workspace;
        this.i = loadObject.i;
        this.v = loadObject.v;
        this.u = loadResponse.userSecurityHash;
        this.t = loadObject.t;

        this.roles = {};
        this.methods = {};

        const objectType = this.workspace.metaPopulation.objectTypeByName[this.t];

        if (loadObject.roles) {
            loadObject.roles.forEach(role => {
                const [name, access] = role;
                const canRead = access.indexOf('r') !== -1;
                const canWrite = access.indexOf('w') !== -1;

                this.roles[`CanRead${name}`] = canRead;
                this.roles[`CanWrite${name}`] = canWrite;

                if (canRead) {
                    const roleType = objectType.roleTypeByName[name];
                    let value = role[2];

                    if (!roleType.objectType) {
                        console.debug(roleType);
                    }

                    if (value && roleType.objectType.isUnit && roleType.objectType.name === 'DateTime') {
                        value = new Date(value as string);
                    }
                    this.roles[name] = value;
                }

            });
        }

        if (loadObject.methods) {
            loadObject.methods.forEach(method => {
                const [name, access] = method;
                const canExecute = access.indexOf('x') !== -1;

                this.methods[`CanExecute${name}`] = canExecute;
            });
        }
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

    get objectType(): ObjectType {
        return this.workspace.metaPopulation.objectTypeByName[this.t];
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
