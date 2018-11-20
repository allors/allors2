import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';

import { SessionService, ErrorService, AllorsRefreshService } from '../../../../angular';
import { AllorsMaterialDialogService } from '../../services/dialog';
import { DeleteAction } from './DeleteAction';

@Injectable()
export class DeleteService {

  constructor(
    public refreshService: AllorsRefreshService,
    public errorService: ErrorService,
    public dialogService: AllorsMaterialDialogService,
    public snackBar: MatSnackBar
  ) { }

  action(sessionService: SessionService) {
    return new DeleteAction(this, sessionService);
  }

}
