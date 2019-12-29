import { Injectable } from '@angular/core';
import { Subject, Observable, merge } from 'rxjs';
import { map, filter, scan, debounceTime } from 'rxjs/operators';

@Injectable()
export class AllorsBarcodeService {

  scan$: Observable<string>;

  private keypressSubject: Subject<any>;

  private simulateSubject: Subject<any>;

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
          return events.length > 4;
        }),
        map((events) => {
          return events.map(v => v.key).join('').trim();
        }),
      );

    this.scan$ = merge(scanner, this.simulateSubject);
  }

  onKeypress(event: KeyboardEvent) {
    this.keypressSubject.next(event);
  }

  scan(barcode: string) {
    this.simulateSubject.next(barcode);
  }
}
