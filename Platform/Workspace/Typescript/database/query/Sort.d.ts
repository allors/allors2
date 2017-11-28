import { RoleType } from "../../meta";
export declare class Sort {
    roleType: RoleType;
    direction?: "Asc" | "Desc";
    constructor(fields?: Partial<Sort>);
    toJSON(): any;
}
