import { Component, Input, Injectable, Inject, ChangeDetectorRef } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';
import { MdDialogRef } from '@angular/material';
import { TdDialogService, TdAlertDialogComponent } from '@covalent/core';

import { workspace, Response, ResponseError, DerivationError } from '../../../domain';

export function errorDialog(dialogService: TdDialogService, error: Error): MdDialogRef<TdAlertDialogComponent> {
    let title: string = '';
    let message: string = '';

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
      message: message,
      title: title,
      closeButton: 'Close',
    });
  }
