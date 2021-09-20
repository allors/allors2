import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export abstract class AllorsMaterialSideNavService {
  abstract toggle$: Observable<void>;

  abstract open$: Observable<void>;

  abstract close$: Observable<void>;

  abstract toggle(): void;

  abstract open(): void;

  abstract close(): void;
}
