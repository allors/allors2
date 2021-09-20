import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

import { AllorsFocusService } from '@allors/angular/services/core';

@Injectable()
export class AllorsFocusServiceCore extends AllorsFocusService {
  public focus$: Observable<any>;
  private focusSubject: BehaviorSubject<any>;

  constructor() {
    super();

    this.focusSubject = new BehaviorSubject(undefined);
    this.focus$ = this.focusSubject;
  }

  focus(trigger: any): void {
    this.focusSubject.next(trigger);
  }
}
