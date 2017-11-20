import { RoleType } from "@allors/base-meta";
import { Predicate } from "./Predicate";
export declare class Like implements Predicate {
    roleType: RoleType;
    value: any;
    constructor(fields?: Partial<Like>);
    toJSON(): any;
}
