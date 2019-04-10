
import { Injectable } from '@angular/core';

import { RoleType } from './../../../../../framework';
import { Action} from '../../../../../angular';
import { PrintAction } from './PrintAction';
import { PrintConfig } from './print.config';

@Injectable({
  providedIn: 'root',
})
export class PrintService {

  constructor(private config: PrintConfig) {
   }

  print(roleType?: RoleType): Action {
    return new PrintAction(this.config, roleType);
  }

}
