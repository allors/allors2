import { Injectable } from '@angular/core';
import { Locale } from 'date-fns';

@Injectable()
export class DateConfig {
  public locale: Locale;
}
