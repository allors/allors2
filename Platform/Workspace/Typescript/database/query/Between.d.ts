import { RoleType } from "../../meta";
import { Predicate } from "./Predicate";
export declare class Between implements Predicate {
    roleType: RoleType;
    first: any;
    second: any;
    constructor(fields?: Partial<Between>);
    toJSON(): any;
}
