import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';

import { Response } from '@allors/protocol/system';
import { ResponseError } from '@allors/protocol/system';

@Component({
  templateUrl: 'errordialog.component.html',
})
export class AllorsMaterialErrorDialogComponent {

  error: ResponseError;
  response: Response;

  isAccessError: boolean;
  isVersionError: boolean;
  isMissingError: boolean;
  isDerivationError: boolean;
  isOtherError: boolean;

  title: string;

  constructor(public dialogRef: MatDialogRef<AllorsMaterialErrorDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: any) {

    this.error = data.error;
    this.response = this.error.response;

    if (this.response.accessErrors && this.response.accessErrors.length > 0) {
      this.isAccessError = true;
      this.title = 'Access Error';
    } else if (this.response.versionErrors && this.response.versionErrors.length > 0) {
      this.isVersionError = true;
      this.title = 'Concurrency Error';
    } else if (this.response.missingErrors && this.response.missingErrors.length > 0) {
      this.isMissingError = true;
      this.title = 'Missing Error';
      // this.message = '';
    } else if (this.response.derivationErrors && this.response.derivationErrors.length > 0) {
      this.isDerivationError = true;
      this.title = 'Derivation Errors';
    } else {
      this.isOtherError = true;
      this.title = 'Error';
    }
  }

  close(): void {
    this.dialogRef.close();
  }
}

