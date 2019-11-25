import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MediaService } from '../../../../../../angular';

import { DialogData } from './dialog.data';

import { Media } from '../../../../../../domain';

@Component({
  templateUrl: './file-preview.component.html',
})
export class FilePreviewComponent {

  media: Media;

  constructor(
    public dialogRef: MatDialogRef<FilePreviewComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private mediaService: MediaService
  ) {
    this.media = data.media;
  }

  get src(): string {
    if (this.media.InDataUri) {
      return this.media.InDataUri;
    } else if (this.media.UniqueId) {
      return this.mediaService.url(this.media);
    }
  }
}
