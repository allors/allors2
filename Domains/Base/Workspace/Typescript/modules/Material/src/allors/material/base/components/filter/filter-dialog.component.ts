import { MatDialogRef } from '@angular/material';
import { Component } from '@angular/core';

@Component({
  templateUrl: 'filter-dialog.component.html',
})
export class AllorsMaterialFilterDialogComponent {

  constructor(public dialogRef: MatDialogRef<AllorsMaterialFilterDialogComponent>) {
  }
}


