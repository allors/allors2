import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { DialogConfig } from './dialog.config';

@Injectable()
export abstract class AllorsMaterialDialogService {
  abstract alert(config: DialogConfig): Observable<any>;

  abstract confirm(config: DialogConfig): Observable<boolean>;

  abstract prompt(config: DialogConfig): Observable<string>;
}
