import { SessionObject } from "@allors/framework";
import { Part } from './Part.g';
export declare class SubAssembly extends SessionObject implements Part {
    readonly CanReadUniqueId: boolean;
    readonly CanWriteUniqueId: boolean;
    UniqueId: string;
}
