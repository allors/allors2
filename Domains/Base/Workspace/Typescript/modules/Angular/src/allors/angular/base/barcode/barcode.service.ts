import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { bufferTime, map, filter, tap } from 'rxjs/operators';

@Injectable()
export class AllorsBarcodeService {

    scan$: Observable<string>;

    // TODO: put in its own service
    keypressSubject: Subject<any>;

    constructor() {
        this.keypressSubject = new Subject();

        this.scan$ = this.keypressSubject
            .pipe(
                map((v) => v.key),
                bufferTime(50),
                // check that last character is a CR/LF or TAB (whitespace)
                filter((v) => v.length > 1 && v[v.length - 1] && v[v.length - 1].trim().length === 0),
                map((v) => v.join('').trim()),
            );

        // this.scan$.subscribe((v) => alert(v));
    }
}
