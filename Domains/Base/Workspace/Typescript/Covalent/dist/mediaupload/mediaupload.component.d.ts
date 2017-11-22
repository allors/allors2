import { EventEmitter } from "@angular/core";
import { Field } from "@allors/base-angular";
import { Media } from "@allors/generated";
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
