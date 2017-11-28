import { SessionObject } from "@allors/framework";
import { Enumeration } from './Enumeration.g';
import { VatRate } from './VatRate.g';
import { OrganisationGlAccount } from './OrganisationGlAccount.g';
import { LocalisedText } from './LocalisedText.g';
export declare class VatRegime extends SessionObject implements Enumeration {
    readonly CanReadVatRate: boolean;
    readonly CanWriteVatRate: boolean;
    VatRate: VatRate;
    readonly CanReadGeneralLedgerAccount: boolean;
    readonly CanWriteGeneralLedgerAccount: boolean;
    GeneralLedgerAccount: OrganisationGlAccount;
    readonly CanReadLocalisedNames: boolean;
    readonly CanWriteLocalisedNames: boolean;
    LocalisedNames: LocalisedText[];
    AddLocalisedName(value: LocalisedText): void;
    RemoveLocalisedName(value: LocalisedText): void;
    readonly CanReadName: boolean;
    readonly CanWriteName: boolean;
    Name: string;
    readonly CanReadIsActive: boolean;
    readonly CanWriteIsActive: boolean;
    IsActive: boolean;
    readonly CanReadUniqueId: boolean;
    readonly CanWriteUniqueId: boolean;
    UniqueId: string;
}
