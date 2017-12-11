import { Component, EventEmitter, Input, Output } from "@angular/core";

import { Field } from "../../../../../angular";
import { Media } from "../../../../../domain";
import { ISession, ISessionObject } from "../../../../../framework";

@Component({
  selector: "a-td-image",
  styleUrls: ["./image.component.scss"],
  templateUrl: "./image.component.html",
})
export class ImageComponent extends Field {
  @Output() public onChange: EventEmitter<Field> = new EventEmitter<Field>();

  @Input() public accept: string = "image/*";

  public file: File;

  get media(): Media {
    return this.model;
  }

  get fieldValue(): string {
    return this.media ? "1 image" : "";
  }

  get src(): string {
    if (this.media) {
      if (this.media.InDataUri) {
        return this.media.InDataUri;
      } else if (this.media.UniqueId) {
        return (
          "http://localhost:5000/Media/Download/" + this.media.UniqueId + "?revision=" + this.media.Revision
        );
      }
    }
  }

  public delete(): void {
    this.model = undefined;
    this.file = undefined;
  }

  public set(file: File): void {
    if (this.ExistObject) {
      if (!this.model) {
        const session: ISession = this.object.session;
        this.model = session.create("Media");
      }
    }

    const reader: FileReader = new FileReader();
    const load: () => void = () => {
      this.media.FileName = file.name;
      this.media.InDataUri = reader.result;
      this.onChange.emit(this);
    };

    reader.addEventListener("load", load, false);
    reader.readAsDataURL(file);
  }
}
