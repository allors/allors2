import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';

import { ErrorService, RefreshService, Action, Context } from '../../../../angular';
import { AllorsMaterialDialogService } from '../../services/dialog';
import { DeleteAction } from './DeleteAction';

@Injectable()
export class DeleteService {

  constructor(
    public refreshService: RefreshService,
    public errorService: ErrorService,
    public dialogService: AllorsMaterialDialogService,
    public snackBar: MatSnackBar
  ) { }

  delete(context: Context): Action {
    return new DeleteAction(this, context);
  }

}
