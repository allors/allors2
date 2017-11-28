import { AssociationType, RoleType } from "../../meta";
import { Predicate } from "./Predicate";
export declare class Exists implements Predicate {
    associationType: AssociationType;
    roleType: RoleType;
    constructor(fields?: Partial<Exists>);
    toJSON(): any;
}
