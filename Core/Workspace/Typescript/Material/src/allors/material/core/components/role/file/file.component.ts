import { Component, EventEmitter, Input, Optional, Output } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';

import { RoleField, MediaService } from '../../../../../angular';
import { Media } from '../../../../../domain';
import { ISession } from '../../../../../framework';

import { FilePreviewComponent } from './preview/file-preview.component';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-file',
  templateUrl: './file.component.html',
})
export class AllorsMaterialFileComponent extends RoleField {

  @Output()
  changed: EventEmitter<RoleField> = new EventEmitter<RoleField>();

  @Input()
  accept = '*/*';

  constructor(
    @Optional() parentForm: NgForm,
    private dialog: MatDialog,
    private mediaService: MediaService,
  ) {
    super(parentForm);
  }

  get media(): Media {
    return this.model;
  }

  set media(value: Media) {
    this.model = value;
  }

  get fieldValue(): string {
    return this.media ? this.media.FileName : '';
  }

  public preview(): void {
    const dialogRef = this.dialog.open(FilePreviewComponent, {
      width: '250px',
      data: { media: this.media }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  public download(): void {
    const url = this.mediaService.url(this.media);
    window.open(url);
  }

  public delete(): void {
    this.model = undefined;
  }

  public onFileInput(event) {

    const files = event.srcElement.files;
    const file = files[0];

    if (this.ExistObject) {
      if (!this.media) {
        const session: ISession = this.object.session;
        this.media = session.create('Media') as Media;
        this.media.InType = file.type;
      }
    }

    const reader: FileReader = new FileReader();
    const load: () => void = () => {
      this.media.FileName = file.name;
      this.media.InDataUri = reader.result as string;
      this.changed.emit(this);
    };

    reader.addEventListener('load', load, false);
    reader.readAsDataURL(file);
  }
}
