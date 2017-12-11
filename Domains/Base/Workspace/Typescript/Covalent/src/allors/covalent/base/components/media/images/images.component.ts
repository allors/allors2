import { Component, EventEmitter, Input, Output } from "@angular/core";

import { Field } from "../../../../../angular";
import { Media } from "../../../../../domain";
import { ISession, ISessionObject } from "../../../../../framework";

@Component({
  selector: "a-td-images",
  styleUrls: ["./images.component.scss"],
  templateUrl: "./images.component.html",
})
export class ImagesComponent extends Field {
  @Output()
  public onChange: EventEmitter<Field> = new EventEmitter<Field>();

  @Input() public accept: string = "image/*";

  public files: File[];

  get medias(): Media[] {
    return this.model;
  }

  get fieldValue(): string {
    if (this.medias && this.medias.length > 0) {
      return this.medias.length + " image" + (this.medias.length > 1 ? "s" : "");
    }

    return "";
  }

  public src(media: Media): string {
    if (media.InDataUri) {
      return media.InDataUri;
    } else if (media.UniqueId) {
      return ("http://localhost:5000/Media/Download/" + media.UniqueId + "?revision=" + media.Revision);
    }
  }

  public deleteAll(): void {
    this.model = undefined;
    this.files = undefined;
  }

  public delete(media: Media): void {
    this.object.remove(this.roleType.name, media);
  }

  public add(fileOrFiles: File | FileList): void {
    const file = fileOrFiles as File;
    const fileList = fileOrFiles as FileList;

    if (fileList.length && fileList.item) {
      for (let i = 0; i < fileList.length; i++) {
        this.addFile(fileList.item(i));
      }
    } else {
      this.addFile(file);
    }
  }

  // TODO: move to RxJS implementation
  private addFile(file: File) {
    const reader: FileReader = new FileReader();
    const load: () => void = () => {
      const media: Media = this.object.session.create("Media") as Media;
      media.FileName = file.name;
      media.InDataUri = reader.result;
      this.object.add(this.roleType.name, media);
    };

    reader.addEventListener("load", load, false);
    reader.readAsDataURL(file);

    this.onChange.emit(this);
  }
}
