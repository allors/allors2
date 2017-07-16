import { Component, Input } from '@angular/core';

import { Injectable, Inject } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';
import { MdSnackBar, MdDialogRef } from '@angular/material';
import { TdDialogService, TdAlertDialogComponent } from '@covalent/core';

import { workspace, Response, ResponseError, DerivationError } from '../../../domain';
import { ErrorService } from '../../../angular';
import { errorDialog } from './errorDialog';

@Injectable()
export class DefaultErrorService extends ErrorService {
  constructor(private dialogService: TdDialogService, private snackBar: MdSnackBar) {
    super();
  }

  message(error: Error): void {
    const message: string = (error as any)._body || error.message;
    this.snackBar.open(message, 'close', { duration: 5000 });
  }

  dialog(error: Error): MdDialogRef<any> {
    return errorDialog(this.dialogService, error);
  }
}
