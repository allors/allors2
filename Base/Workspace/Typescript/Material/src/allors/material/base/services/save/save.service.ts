import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material';

import { AllorsMaterialErrorDialogComponent } from './error';
import { ResponseError } from '../../../../../allors/framework';

@Injectable()
export class SaveService {

  public errorHandler: (error: any) => void;

  constructor(private dialog: MatDialog) {

    this.errorHandler = (error) => {

      if (error instanceof ResponseError) {
        this.dialog.open(AllorsMaterialErrorDialogComponent, {
          data: { error }
        });
      } else {
        throw error;
      }

    };
  }
}
