import { SessionObject } from "@allors/framework";
import { AgreementTerm } from './AgreementTerm.g';
import { TermType } from './TermType.g';
export declare class InvoiceTerm extends SessionObject implements AgreementTerm {
    readonly CanReadTermValue: boolean;
    readonly CanWriteTermValue: boolean;
    TermValue: string;
    readonly CanReadTermType: boolean;
    readonly CanWriteTermType: boolean;
    TermType: TermType;
    readonly CanReadDescription: boolean;
    readonly CanWriteDescription: boolean;
    Description: string;
}
