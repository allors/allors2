import { Observable } from "rxjs/Observable";
import "rxjs/add/observable/empty";
import { ISessionObject, ObjectType, ObjectTyped, RoleType } from "@allors/framework";
import { Scope } from "./Scope";
export interface FilterOptions {
    scope: Scope;
    objectType: ObjectType | ObjectTyped;
    roleTypes: RoleType[];
    existRoletypes?: RoleType[];
    notExistRoletypes?: RoleType[];
}
export declare class Filter {
    private options;
    constructor(options: FilterOptions);
    create(): ((search: string) => Observable<ISessionObject[]>);
}
