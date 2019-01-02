import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';

import { ErrorService, RefreshService, Action, Context } from '../../../../../angular';
import { AllorsMaterialDialogService } from '../../../services/dialog';
import { DeleteAction } from './DeleteAction';

@Injectable()
export class DeleteService {

  constructor(
    private refreshService: RefreshService,
    private errorService: ErrorService,
    private dialogService: AllorsMaterialDialogService,
    private snackBar: MatSnackBar
  ) { }

  delete(context: Context): Action {
    return new DeleteAction(this.refreshService, this.errorService, this.dialogService, this.snackBar, context);
  }

}
