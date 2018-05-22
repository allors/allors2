/* import { ChangeDetectorRef, Component, Inject, Injectable, Input } from '@angular/core';
import { MatDialogRef } from '@angular/material';

import { DerivationError, Response, ResponseError } from '../../../../framework';

export function errorDialog(dialogService: TdDialogService, error: Error): MatDialogRef<TdAlertDialogComponent> {
  let title = '';
  let message = '';

  if (error instanceof ResponseError) {
    const responseError: ResponseError = error;
    const response: Response = error.response;

    if (response.accessErrors && response.accessErrors.length > 0) {
      title = 'Access Error';
      message = 'You do not have the required rights.';
    } else if ((response.versionErrors && response.versionErrors.length > 0) ||
      (response.missingErrors && response.missingErrors.length > 0)) {
      title = 'Concurrency Error';
      message += 'Modifications were detected since last access.';
    } else if (response.derivationErrors && response.derivationErrors.length > 0) {
      title = 'Derivation Errors';
      response.derivationErrors.map((derivationError: DerivationError) => {
        message += `\n* ${derivationError.m}`;
      });
    } else {
      title = 'Error';
      message = responseError.message;
    }
  } else {
    title = 'Error';
    if (error.message) {
      message = error.message;
    } else {
      message = JSON.stringify(error);
    }
  }

  return dialogService.openAlert({
    message,
    title,
    closeButton: 'Close',
  });
}
*/
