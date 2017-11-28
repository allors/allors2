import { SessionObject, Method } from "@allors/framework";
import { PartyRelationship } from './PartyRelationship.g';
import { Party } from './Party.g';
export declare class SubContractorRelationship extends SessionObject implements PartyRelationship {
    readonly CanReadContractor: boolean;
    readonly CanWriteContractor: boolean;
    Contractor: Party;
    readonly CanReadSubContractor: boolean;
    readonly CanWriteSubContractor: boolean;
    SubContractor: Party;
    readonly CanReadParties: boolean;
    readonly Parties: Party[];
    readonly CanReadFromDate: boolean;
    readonly CanWriteFromDate: boolean;
    FromDate: Date;
    readonly CanReadThroughDate: boolean;
    readonly CanWriteThroughDate: boolean;
    ThroughDate: Date;
    readonly CanExecuteDelete: boolean;
    readonly Delete: Method;
}
