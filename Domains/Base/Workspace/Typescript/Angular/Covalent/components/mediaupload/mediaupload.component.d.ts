import { EventEmitter } from "@angular/core";
import { Media } from "@allors/workspace";
import { Field } from "@allors/base-angular";
export declare class MediaUploadComponent extends Field {
    selected: EventEmitter<string>;
    accept: string;
    file: File;
    readonly media: Media;
    fileName: string;
    delete(): void;
    dropEvent(files: FileList): void;
    selectEvent(file: File): void;
}
