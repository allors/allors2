import { AssociationType, RoleType } from "../../meta";
import { ISessionObject } from "./../../workspace/SessionObject";
import { Predicate } from "./Predicate";
import { Query } from "./Query";
export declare class ContainedIn implements Predicate {
    associationType: AssociationType;
    roleType: RoleType;
    query: Query;
    objects: ISessionObject[];
    constructor(fields?: Partial<ContainedIn>);
    toJSON(): any;
}
