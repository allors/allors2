import { SessionObject } from "@allors/framework";
import { AgreementTerm } from './AgreementTerm.g';
import { Country } from './Country.g';
import { TermType } from './TermType.g';
export declare class IncoTerm extends SessionObject implements AgreementTerm {
    readonly CanReadincoTermCity: boolean;
    readonly CanWriteincoTermCity: boolean;
    incoTermCity: string;
    readonly CanReadIncoTermCountry: boolean;
    readonly CanWriteIncoTermCountry: boolean;
    IncoTermCountry: Country;
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
