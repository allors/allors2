import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable()
export class AllorsFocusService {

    public focus$: Observable<any>;
    private focusSubject: BehaviorSubject<any>;

    constructor() {

      this.focusSubject = new BehaviorSubject(undefined);
      this.focus$ = this.focusSubject;
    }

    public focus(trigger: any) {
        this.focusSubject.next(trigger);
    }
}
