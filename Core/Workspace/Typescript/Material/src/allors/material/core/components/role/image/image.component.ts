import { Component, EventEmitter, Input, Optional, Output } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';

import { RoleField, MediaService } from '../../../../../angular';
import { Media } from '../../../../../domain';
import { ISession } from '../../../../../framework';

import { ImagePreviewComponent } from './preview/image-preview.component';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-image',
  templateUrl: './image.component.html',
})
export class AllorsMaterialImageComponent {

  @Input()
  media: Media;

  constructor(
    @Optional() parentForm: NgForm,
    private dialog: MatDialog,
    private mediaService: MediaService,
  ) {
  }

  public delete(): void {

  }

  public preview(): void {
    const dialogRef = this.dialog.open(ImagePreviewComponent, {
      maxHeight: '80vh',
      maxWidth: '60vw',
      data: { media: this.media }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  get src(): string {
    if (this.media.InDataUri) {
      return this.media.InDataUri;
    } else if (this.media.UniqueId) {
      return this.mediaService.url(this.media);
    }
  }
}
