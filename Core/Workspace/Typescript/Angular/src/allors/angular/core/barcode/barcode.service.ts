import { Injectable } from '@angular/core';
import { Subject, Observable, merge } from 'rxjs';
import { bufferTime, map, filter, distinct, bufferWhen, scan, debounce, debounceTime } from 'rxjs/operators';

@Injectable()
export class AllorsBarcodeService {

  scan$: Observable<string>;

  keypressSubject: Subject<any>;
  simulateSubject: Subject<any>;

  constructor() {
    this.keypressSubject = new Subject();
    this.simulateSubject = new Subject();

    const scanner = this.keypressSubject
      .pipe(
        scan((events: KeyboardEvent[], event: KeyboardEvent) => {
          events = [...events, event].filter(v => event.timeStamp < v.timeStamp + 100);
          return events;
        }, []),
        debounceTime(50),
        filter((events) => {
          console.log(events)
          return events.length > 1;
        }),
        map((events) => {
          return events.map(v => v.key).join('').trim();
        }),
      );

    this.scan$ = merge(scanner, this.simulateSubject);

    //this.scan$.subscribe((v) => alert(v));
  }
}
