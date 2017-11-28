import { SessionObject, Method } from "@allors/framework";
import { Period } from './Period.g';
import { Deletable } from './Deletable.g';
import { Party } from './Party.g';
export declare class CustomerRelationship extends SessionObject implements Period, Deletable {
    readonly CanReadCustomer: boolean;
    readonly CanWriteCustomer: boolean;
    Customer: Party;
    readonly CanReadFromDate: boolean;
    readonly CanWriteFromDate: boolean;
    FromDate: Date;
    readonly CanReadThroughDate: boolean;
    readonly CanWriteThroughDate: boolean;
    ThroughDate: Date;
    readonly CanExecuteDelete: boolean;
    readonly Delete: Method;
}
