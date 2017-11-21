import { ISessionObject } from "@allors/base-domain";
import { RoleType } from "@allors/base-meta";
export declare abstract class Field {
    object: ISessionObject;
    roleType: RoleType;
    assignedDisabled: boolean;
    assignedRequired: boolean;
    assignedLabel: string;
    readonly: boolean;
    hint: string;
    readonly ExistObject: boolean;
    model: any;
    readonly canRead: boolean;
    readonly canWrite: boolean;
    readonly textType: string;
    readonly name: string;
    readonly label: string;
    readonly required: boolean;
    readonly disabled: boolean;
    protected humanize(input: string): string;
}
