import { SessionObject } from "@allors/framework";
import { ProductAssociation } from './ProductAssociation.g';
export declare class ProductConfiguration extends SessionObject implements ProductAssociation {
    readonly CanReadComment: boolean;
    readonly CanWriteComment: boolean;
    Comment: string;
    readonly CanReadFromDate: boolean;
    readonly CanWriteFromDate: boolean;
    FromDate: Date;
    readonly CanReadThroughDate: boolean;
    readonly CanWriteThroughDate: boolean;
    ThroughDate: Date;
}
