import { SessionObject, Method } from "@allors/framework";
import { Period } from './Period.g';
import { Deletable } from './Deletable.g';
import { Party } from './Party.g';
export interface PartyRelationship extends SessionObject, Period, Deletable {
    Parties: Party[];
    CanExecuteDelete: boolean;
    Delete: Method;
}
