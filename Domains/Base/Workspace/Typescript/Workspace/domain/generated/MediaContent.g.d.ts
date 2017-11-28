import { SessionObject, Method } from "@allors/framework";
import { Deletable } from './Deletable.g';
export declare class MediaContent extends SessionObject implements Deletable {
    readonly CanReadType: boolean;
    readonly CanWriteType: boolean;
    Type: string;
    readonly CanReadData: boolean;
    readonly CanWriteData: boolean;
    Data: any;
    readonly CanExecuteDelete: boolean;
    readonly Delete: Method;
}
