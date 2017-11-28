import { SessionObject, Method } from "@allors/framework";
import { PartyRelationship } from './PartyRelationship.g';
import { Organisation } from './Organisation.g';
import { OrganisationUnit } from './OrganisationUnit.g';
import { Party } from './Party.g';
export declare class OrganisationRollUp extends SessionObject implements PartyRelationship {
    readonly CanReadParent: boolean;
    readonly CanWriteParent: boolean;
    Parent: Organisation;
    readonly CanReadRollupKind: boolean;
    readonly CanWriteRollupKind: boolean;
    RollupKind: OrganisationUnit;
    readonly CanReadChild: boolean;
    readonly CanWriteChild: boolean;
    Child: Organisation;
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
