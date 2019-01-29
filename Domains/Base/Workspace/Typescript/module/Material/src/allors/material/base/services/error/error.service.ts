import { Injectable } from '@angular/core';
import { Location } from '@angular/common';
import { MatDialog } from '@angular/material';

import { Observable } from 'rxjs';

import { ErrorService, LoggingService } from '../../../../angular';
import { ResponseError } from '../../../../framework';

import { AllorsMaterialErrorDialogComponent } from '../../components/errordialog/errordialog.module';

@Injectable()
export class AllorsMaterialDefaultErrorService extends ErrorService {
  public handler: (error: any) => void;

  constructor(
    private loggingService: LoggingService,
    private location: Location,
    private dialog: MatDialog) {

    super();

    this.handler = (error) => {
      this.handle(error).subscribe(() => this.location.back());
    };
  }

  public handle(error: Error): Observable<any> {

    this.loggingService.error(error);

    const dialogRef = this.dialog.open(AllorsMaterialErrorDialogComponent, {
      data: { error }
    });

    return dialogRef.afterClosed();
  }
}
