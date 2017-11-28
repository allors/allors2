import { SessionObject, Method } from "@allors/framework";
import { PartyRelationship } from './PartyRelationship.g';
import { Person } from './Person.g';
import { Organisation } from './Organisation.g';
import { Party } from './Party.g';
export declare class ProfessionalServicesRelationship extends SessionObject implements PartyRelationship {
    readonly CanReadProfessional: boolean;
    readonly CanWriteProfessional: boolean;
    Professional: Person;
    readonly CanReadProfessionalServicesProvider: boolean;
    readonly CanWriteProfessionalServicesProvider: boolean;
    ProfessionalServicesProvider: Organisation;
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
