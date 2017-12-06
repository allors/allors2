import { Component, EventEmitter, Input, Output } from "@angular/core";

import { Field } from "../../../../angular";
import { Media } from "../../../../domain";
import { ISession, ISessionObject } from "../../../../framework";

@Component({
  selector: "a-mat-media-upload",
  template: `
<div fxLayout="row">
  <mat-input-container tdFileDrop (fileDrop)="dropEvent($event)" flex>
      <input matInput [placeholder]="label" [value]="fileName" [disabled]="!this.ExistObject"/>
  </mat-input-container>

  <button mat-icon-button *ngIf="media" (click)="delete()" (keyup.enter)="delete()">
    <mat-icon>cancel</mat-icon>
  </button>

  <td-file-input #fileInput [(ngModel)]="file" color="primary" (select)="selectEvent($event)" accept="accept" [disabled]="!canWrite" [required]="required">
    <mat-icon>attach_file</mat-icon><span>Choose a file ...</span>
  </td-file-input>
</div>
`,
})
export class MediaUploadComponent extends Field {

  @Output()
  public selected: EventEmitter<string> = new EventEmitter<string>();

  @Input()
  public accept: string;

  public file: File;

  get media(): Media {
    return this.model;
  }

  get fileName(): string {
    return this.media ? this.media.FileName : undefined;
  }

  set fileName(value: string) {
    if (this.ExistObject) {
      this.media.FileName = value;
    }
  }

  delete(): void {
    this.model = undefined;
  }

  dropEvent(files: FileList): void {
    this.selectEvent(files[0]);
  }

  selectEvent(file: File): void {
    if (this.ExistObject) {
      if (!this.model) {
        const session: ISession = this.object.session;
        this.model = session.create("Media");
      }
    }

    const reader: FileReader = new FileReader();
    const load: () => void =  () => {
      this.media.FileName = file.name;
      this.media.InDataUri = reader.result;
      this.selected.emit(this.media.id);
    };

    reader.addEventListener("load", load, false);
    reader.readAsDataURL(file);
  }
}
