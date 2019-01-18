import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';

import { ErrorService, RefreshService, Action, Context } from '../../../../../angular';
import { DeleteAction as MethodAction } from './MethodAction';
import { MethodType } from '../../../../../framework';

import { MethodConfig } from './MethodConfig';

@Injectable()
export class MethodService {

  constructor(
    private refreshService: RefreshService,
    private errorService: ErrorService,
    private snackBar: MatSnackBar
  ) { }

  create(context: Context, methodType: MethodType, config?: MethodConfig): Action {
    return new MethodAction(this.refreshService, this.errorService, this.snackBar, context, methodType, config);
  }

}
