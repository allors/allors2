import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

import { RefreshService, Context, Action } from '@allors/angular/core';


import { DeleteAction } from './DeleteAction';
import { AllorsMaterialDialogService, SaveService } from '@allors/angular/material/core';

@Injectable({
  providedIn: 'root',
})
export class DeleteService {
  constructor(
    private refreshService: RefreshService,
    private dialogService: AllorsMaterialDialogService,
    private saveService: SaveService,
    private snackBar: MatSnackBar,
  ) {}

  delete(context: Context): Action {
    return new DeleteAction(this.refreshService, this.dialogService, this.saveService, this.snackBar, context);
  }
}
