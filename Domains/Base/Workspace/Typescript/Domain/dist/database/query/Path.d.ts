import { AssociationType, RoleType } from "@allors/base-meta";
export declare class Path {
    step: AssociationType | RoleType;
    next: Path;
    constructor(fields?: Partial<Path>);
    toJSON(): any;
}
