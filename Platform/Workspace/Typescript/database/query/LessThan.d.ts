import { RoleType } from "../../meta";
import { Predicate } from "./Predicate";
export declare class LessThan implements Predicate {
    roleType: RoleType;
    value: any;
    constructor(fields?: Partial<LessThan>);
    toJSON(): any;
}
