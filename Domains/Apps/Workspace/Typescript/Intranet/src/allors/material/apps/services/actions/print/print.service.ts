import { Injectable } from '@angular/core';

import { Action, ErrorService } from '../../../../../angular';
import { PrintAction } from './PrintAction';
import { PrintConfig } from './print.config';

@Injectable({
  providedIn: 'root',
})
export class PrintService {

  constructor(private config: PrintConfig, private errorService: ErrorService) {
   }

  print(): Action {
    return new PrintAction(this.config, this.errorService);
  }

}
