import { Component, Inject } from '@angular/core';
import {  MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogData } from './dialog.data';

@Component({
  templateUrl: './file-preview.component.html',
})
export class FilePreviewComponent {
  constructor(
    public dialogRef: MatDialogRef<FilePreviewComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData) { }
}
