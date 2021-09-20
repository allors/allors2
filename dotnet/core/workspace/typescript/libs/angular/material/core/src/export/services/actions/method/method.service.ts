import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

import { MethodType } from '@allors/meta/system';
import { RefreshService, Context } from '@allors/angular/services/core';
import { Action } from '@allors/angular/core';
import { SaveService } from '@allors/angular/material/services/core';

import { MethodAction } from './MethodAction';
import { MethodConfig } from './MethodConfig';

@Injectable({
  providedIn: 'root',
})
export class MethodService {
  constructor(private refreshService: RefreshService, private saveService: SaveService, private snackBar: MatSnackBar) {}

  create(context: Context, methodType: MethodType, config?: MethodConfig): Action {
    return new MethodAction(this.refreshService, this.snackBar, context, this.saveService, methodType, config);
  }
}
