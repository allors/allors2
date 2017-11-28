import { SessionObject, Method } from "@allors/framework";
import { PartyRelationship } from './PartyRelationship.g';
import { Person } from './Person.g';
import { Organisation } from './Organisation.g';
import { OrganisationContactKind } from './OrganisationContactKind.g';
import { Party } from './Party.g';
export declare class OrganisationContactRelationship extends SessionObject implements PartyRelationship {
    readonly CanReadContact: boolean;
    readonly CanWriteContact: boolean;
    Contact: Person;
    readonly CanReadOrganisation: boolean;
    readonly CanWriteOrganisation: boolean;
    Organisation: Organisation;
    readonly CanReadContactKinds: boolean;
    readonly CanWriteContactKinds: boolean;
    ContactKinds: OrganisationContactKind[];
    AddContactKind(value: OrganisationContactKind): void;
    RemoveContactKind(value: OrganisationContactKind): void;
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
