import { SessionObject } from "@allors/framework";
import { TermType } from './TermType.g';
export interface AgreementTerm extends SessionObject {
    TermValue: string;
    TermType: TermType;
    Description: string;
}
