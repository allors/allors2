import { SessionObject } from "@allors/framework";
export declare class GeneralLedgerAccountGroup extends SessionObject {
    readonly CanReadParent: boolean;
    readonly CanWriteParent: boolean;
    Parent: GeneralLedgerAccountGroup;
    readonly CanReadDescription: boolean;
    readonly CanWriteDescription: boolean;
    Description: string;
}
