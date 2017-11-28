import { SessionObject, Method } from "@allors/framework";
import { Period } from './Period.g';
import { Deletable } from './Deletable.g';
import { Organisation } from './Organisation.g';
export declare class SupplierRelationship extends SessionObject implements Period, Deletable {
    readonly CanReadSupplier: boolean;
    readonly CanWriteSupplier: boolean;
    Supplier: Organisation;
    readonly CanReadFromDate: boolean;
    readonly CanWriteFromDate: boolean;
    FromDate: Date;
    readonly CanReadThroughDate: boolean;
    readonly CanWriteThroughDate: boolean;
    ThroughDate: Date;
    readonly CanExecuteDelete: boolean;
    readonly Delete: Method;
}
