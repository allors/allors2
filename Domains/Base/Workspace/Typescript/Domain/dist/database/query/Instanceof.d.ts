import { AssociationType, ObjectType, RoleType } from "@allors/base-meta";
import { Predicate } from "./Predicate";
export declare class Instanceof implements Predicate {
    associationType: AssociationType;
    roleType: RoleType;
    objectType: ObjectType;
    constructor(fields?: Partial<Instanceof>);
    toJSON(): any;
}
