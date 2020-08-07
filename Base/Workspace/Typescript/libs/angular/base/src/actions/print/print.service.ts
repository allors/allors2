
import { Injectable } from '@angular/core';

import { RoleType } from '@allors/meta/system';
import { Action } from '@allors/angular/core';

import { PrintAction } from './PrintAction';
import { PrintConfig } from './print.config';

@Injectable({
  providedIn: 'root',
})
export class PrintService {

  constructor(public config: PrintConfig) {
   }

  print(roleType?: RoleType): Action {
    return new PrintAction(this.config, roleType);
  }

}
