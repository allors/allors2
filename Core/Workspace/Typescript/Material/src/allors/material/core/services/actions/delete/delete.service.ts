import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

import { Action, Context } from '../../../../../angular';
import { RefreshService } from '../../../../../angular/core/refresh';
import { AllorsMaterialDialogService } from '../../../services/dialog';
import { DeleteAction } from './DeleteAction';
import { SaveService } from '../../save/save.service';

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
