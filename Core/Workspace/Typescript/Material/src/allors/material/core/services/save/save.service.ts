import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { AllorsMaterialErrorDialogComponent } from './error/errordialog.component';
import { ResponseError } from '../../../../../allors/framework';

@Injectable({
  providedIn: 'root',
})
export class SaveService {

  public errorHandler: (error: any) => void;

  constructor(private dialog: MatDialog) {

    this.errorHandler = (error) => {

      if (error instanceof ResponseError) {
        this.dialog.open(AllorsMaterialErrorDialogComponent, {
          data: { error },
          maxHeight: '90vh'
        });
      } else {
        throw error;
      }

    };
  }
}
