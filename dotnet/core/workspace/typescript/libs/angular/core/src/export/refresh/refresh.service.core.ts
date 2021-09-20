import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';

import { RefreshService } from '@allors/angular/services/core';

@Injectable()
export class RefreshServiceCore extends RefreshService {
  refresh$: Observable<Date>;
  private refreshSubject$: BehaviorSubject<Date>;

  constructor() {
    super();
    this.refresh$ = this.refreshSubject$ = new BehaviorSubject(new Date());
  }

  refresh() {
    this.refreshSubject$.next(new Date());
  }
}
