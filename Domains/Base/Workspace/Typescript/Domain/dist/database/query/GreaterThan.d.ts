import { RoleType } from "@allors/base-meta";
import { Predicate } from "./Predicate";
export declare class GreaterThan implements Predicate {
    roleType: RoleType;
    value: any;
    constructor(fields?: Partial<GreaterThan>);
    toJSON(): any;
}
