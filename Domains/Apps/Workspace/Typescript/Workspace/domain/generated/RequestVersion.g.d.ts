import { SessionObject } from "@allors/framework";
import { Version } from './Version.g';
import { RequestState } from './RequestState.g';
import { RequestItem } from './RequestItem.g';
import { RespondingParty } from './RespondingParty.g';
import { Party } from './Party.g';
import { Currency } from './Currency.g';
import { ContactMechanism } from './ContactMechanism.g';
export interface RequestVersion extends SessionObject, Version {
    RequestState: RequestState;
    InternalComment: string;
    Description: string;
    RequestDate: Date;
    RequiredResponseDate: Date;
    RequestItems: RequestItem[];
    AddRequestItem(value: RequestItem): any;
    RemoveRequestItem(value: RequestItem): any;
    RequestNumber: string;
    RespondingParties: RespondingParty[];
    AddRespondingParty(value: RespondingParty): any;
    RemoveRespondingParty(value: RespondingParty): any;
    Originator: Party;
    Currency: Currency;
    FullfillContactMechanism: ContactMechanism;
    EmailAddress: string;
    TelephoneNumber: string;
    TelephoneCountryCode: string;
}
