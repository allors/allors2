import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export abstract class AllorsBarcodeService {
  abstract readonly scan$: Observable<string>;

  abstract onKeypress(event: KeyboardEvent);

  abstract scan(barcode: string);
}
