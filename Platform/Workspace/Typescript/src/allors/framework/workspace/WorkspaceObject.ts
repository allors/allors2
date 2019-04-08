import { ObjectType } from '../meta';
import { SyncResponse, SyncResponseObject } from '../protocol/sync/SyncResponse';
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
    public workspace: IWorkspace;
    public roles: any;
    public methods: any;

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
            loadObject.roles.forEach((role) => {
                const [name, access] = role;
                const canRead = access.indexOf('r') !== -1;
                const canWrite = access.indexOf('w') !== -1;

                this.roles[`CanRead${name}`] = canRead;
                this.roles[`CanWrite${name}`] = canWrite;

                if (canRead) {
                    const value = role[2];
                    this.roles[name] = value;
                }

            });
        }

        if (loadObject.methods) {
            loadObject.methods.forEach((method) => {
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

    public canRead(roleTypeName: string): boolean {
        return this.roles[`CanRead${roleTypeName}`];
    }

    public canWrite(roleTypeName: string): boolean {
        return this.roles[`CanWrite${roleTypeName}`];
    }

    public canExecute(methodName: string): boolean {
        return this.methods[`CanExecute${methodName}`];
    }
}
