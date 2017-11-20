import { AssociationType, RoleType } from "@allors/base-meta";
import { ISessionObject } from "./../../workspace/SessionObject";
import { Predicate } from "./Predicate";
export declare class Contains implements Predicate {
    associationType: AssociationType;
    roleType: RoleType;
    object: ISessionObject;
    constructor(fields?: Partial<Contains>);
    toJSON(): any;
}
