import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';

@Component({
  selector: 'newgood-dialog',
  templateUrl: './newgood-dialog-component.html',
})

export class NewGoodDialogComponent {
  constructor(public dialogRef: MatDialogRef<NewGoodDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: any) {}

  public onCancelClick(): void {
    this.dialogRef.close();
  }
}
