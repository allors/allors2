import { Injectable } from '@angular/core';

import { Action, Context, MediaService } from '../../../../../angular';
import { PrintAction } from './PrintAction';

@Injectable({
  providedIn: 'root',
})
export class PrintService {

  constructor(
    private mediaService: MediaService
  ) { }

  print(): Action {
    return new PrintAction(this.mediaService);
  }

}
