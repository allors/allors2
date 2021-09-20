import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { ResponseError } from '@allors/protocol/system';
import { SaveService } from '@allors/angular/material/services/core';

import { AllorsMaterialErrorDialogComponent } from './error/errordialog.component';

@Injectable()
export class SaveServiceCore extends SaveService {

  public errorHandler: (error: any) => void;

  constructor(private dialog: MatDialog) {
    super();

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
