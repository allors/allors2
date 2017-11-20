import { AssociationType, RoleType } from "@allors/base-meta";
import { Predicate } from "./Predicate";
export declare class Equals implements Predicate {
    associationType: AssociationType;
    roleType: RoleType;
    value: any;
    constructor(fields?: Partial<Equals>);
    toJSON(): any;
}
