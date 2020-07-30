import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

import { Action, Context } from '../../../../../angular';
import { RefreshService } from '../../../../../angular/core/services/refresh.service';
import { MethodAction } from './MethodAction';
import { MethodType } from '../../../../../framework';

import { MethodConfig } from './MethodConfig';
import { SaveService } from '../../save';

@Injectable({
  providedIn: 'root',
})
export class MethodService {
  constructor(private refreshService: RefreshService, private saveService: SaveService, private snackBar: MatSnackBar) {}

  create(context: Context, methodType: MethodType, config?: MethodConfig): Action {
    return new MethodAction(this.refreshService, this.snackBar, context, this.saveService, methodType, config);
  }
}
