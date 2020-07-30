import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class RefreshService {
  refresh$: Observable<Date>;
  private refreshSubject$: BehaviorSubject<Date>;

  constructor() {
    this.refresh$ = this.refreshSubject$ = new BehaviorSubject(new Date());
  }

  refresh() {
    this.refreshSubject$.next(new Date());
  }
}
