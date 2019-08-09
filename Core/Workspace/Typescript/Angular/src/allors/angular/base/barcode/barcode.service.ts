import { Injectable } from '@angular/core';
import { Subject, Observable, BehaviorSubject, merge } from 'rxjs';
import { bufferTime, map, filter, tap, combineLatest } from 'rxjs/operators';

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
                map((v) => v.key),
                bufferTime(50),
                // check that last character is a CR/LF or TAB (whitespace)
                filter((v) => v.length > 1 && v[v.length - 1] && v[v.length - 1].trim().length === 0),
                map((v) => v.join('').trim()),
            );

        this.scan$ = merge(scanner, this.simulateSubject);

        // this.scan$.subscribe((v) => alert(v));
    }
}
