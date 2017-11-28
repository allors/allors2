import { SessionObject } from "@allors/framework";
import { PriceComponent } from './PriceComponent.g';
export declare class SurchargeComponent extends SessionObject implements PriceComponent {
    readonly CanReadFromDate: boolean;
    readonly CanWriteFromDate: boolean;
    FromDate: Date;
    readonly CanReadThroughDate: boolean;
    readonly CanWriteThroughDate: boolean;
    ThroughDate: Date;
    readonly CanReadComment: boolean;
    readonly CanWriteComment: boolean;
    Comment: string;
}
