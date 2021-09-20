import { Injectable } from '@angular/core';
import { DateConfig } from './date.config';

import { DateService } from '@allors/angular/services/core';

@Injectable()
export class DateServiceCore extends DateService {
  public locale: Locale;

  constructor(dateConfig: DateConfig) {
    super();
    this.locale = dateConfig.locale;
  }
}
