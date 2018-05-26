import { Component, Inject, Injectable, Input } from '@angular/core';
import { MatDialogRef, MatSnackBar, MatDialog } from '@angular/material';

import { ErrorService, LoggingService } from '../../../../angular';
import { DerivationError, Response, ResponseError } from '../../../../framework';

import { AllorsMaterialErrorDialogComponent } from '../../components/errordialog/errordialog.module';
import { Observable } from 'rxjs';

@Injectable()
export class AllorsMaterialDefaultErrorService extends ErrorService {
  constructor(private loggingService: LoggingService, private dialog: MatDialog) {
    super();
  }

  public handle(error: Error): Observable<any> {

    this.loggingService.error(error);

    const dialogRef = this.dialog.open(AllorsMaterialErrorDialogComponent, {
      data: { error }
    });

    return dialogRef.afterClosed();
  }
}
