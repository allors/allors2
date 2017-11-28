import { RoleType } from "../../meta";
import { Predicate } from "./Predicate";
export declare class GreaterThan implements Predicate {
    roleType: RoleType;
    value: any;
    constructor(fields?: Partial<GreaterThan>);
    toJSON(): any;
}
