import { Injectable } from '@angular/core';

import { Action, Context, MediaService } from '../../../../../angular';
import { PrintAction } from './PrintAction';
import { PrintConfig } from './print.config';

@Injectable({
  providedIn: 'root',
})
export class PrintService {

  constructor(private config: PrintConfig) {
   }

  print(): Action {
    return new PrintAction(this.config);
  }

}
