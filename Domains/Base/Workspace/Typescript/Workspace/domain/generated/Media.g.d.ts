import { SessionObject, Method } from "@allors/framework";
import { UniquelyIdentifiable } from './UniquelyIdentifiable.g';
import { Deletable } from './Deletable.g';
import { MediaContent } from './MediaContent.g';
export declare class Media extends SessionObject implements UniquelyIdentifiable, Deletable {
    readonly CanReadRevision: boolean;
    readonly Revision: string;
    readonly CanReadMediaContent: boolean;
    readonly CanWriteMediaContent: boolean;
    MediaContent: MediaContent;
    readonly CanReadInData: boolean;
    readonly CanWriteInData: boolean;
    InData: any;
    readonly CanReadInDataUri: boolean;
    readonly CanWriteInDataUri: boolean;
    InDataUri: string;
    readonly CanReadFileName: boolean;
    readonly CanWriteFileName: boolean;
    FileName: string;
    readonly CanReadType: boolean;
    readonly Type: string;
    readonly CanReadUniqueId: boolean;
    readonly CanWriteUniqueId: boolean;
    UniqueId: string;
    readonly CanExecuteDelete: boolean;
    readonly Delete: Method;
}
