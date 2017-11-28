import { SessionObject, Method } from "@allors/framework";
import { Deletable } from './Deletable.g';
import { PriceComponent } from './PriceComponent.g';
export declare class BasePrice extends SessionObject implements Deletable, PriceComponent {
    readonly CanReadFromDate: boolean;
    readonly CanWriteFromDate: boolean;
    FromDate: Date;
    readonly CanReadThroughDate: boolean;
    readonly CanWriteThroughDate: boolean;
    ThroughDate: Date;
    readonly CanReadComment: boolean;
    readonly CanWriteComment: boolean;
    Comment: string;
    readonly CanExecuteDelete: boolean;
    readonly Delete: Method;
}
