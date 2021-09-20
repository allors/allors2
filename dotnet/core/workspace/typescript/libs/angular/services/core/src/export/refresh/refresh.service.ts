import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export abstract class RefreshService {
  refresh$: Observable<Date>;
  
  abstract refresh(): void;
}
