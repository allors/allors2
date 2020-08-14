
import { Injectable } from '@angular/core';

import { RoleType } from '@allors/meta/system';
import { Action } from '@allors/angular/core';

import { PrintAction } from './PrintAction';


export class PrintConfig {
  url: string;
}


@Injectable()
export class PrintService {

  constructor(public config: PrintConfig) {
   }

  print(roleType?: RoleType): Action {
    return new PrintAction(this.config, roleType);
  }

}
