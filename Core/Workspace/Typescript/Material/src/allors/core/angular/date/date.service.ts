import { Injectable } from '@angular/core';
import { DateConfig } from './date.config';

@Injectable({
  providedIn: 'root',
})
export class DateService {
  public locale: Locale;

  constructor(dateConfig: DateConfig) {
    this.locale = dateConfig.locale;
  }
}
