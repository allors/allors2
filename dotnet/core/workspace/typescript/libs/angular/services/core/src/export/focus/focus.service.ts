import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable()
export abstract class AllorsFocusService {
  abstract readonly focus$: Observable<any>;

  abstract focus(trigger: any): void
}
