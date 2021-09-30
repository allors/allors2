import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export abstract class DateService {
  abstract readonly locale: Locale;
}
