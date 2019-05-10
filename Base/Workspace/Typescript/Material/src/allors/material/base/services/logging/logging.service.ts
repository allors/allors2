import { Injectable } from '@angular/core';
import { LoggingService } from '../../../../angular';

import { LoggingConfig } from './logging.config';

@Injectable()
export class DefaultLoggingService extends LoggingService {

  constructor(private config: LoggingConfig) {
    super();
  }

  public trace(logObject: any) {

    if (this.config.console) {
      // tslint:disable-next-line:no-console
      console.trace(logObject);
    }
  }

  public debug(logObject: any) {
    if (this.config.console) {
      // tslint:disable-next-line:no-console
      console.debug(logObject);
    }
  }

  public info(logObject: any) {
    if (this.config.console) {
      // tslint:disable-next-line:no-console
      console.info(logObject);
    }
  }

  public warn(logObject: any) {
    if (this.config.console) {
      console.warn(logObject);
    }
  }

  public error(logObject: any) {
    if (this.config.console) {
      console.error(logObject);
    }
  }

  public fatal(logObject: any) {
    if (this.config.console) {
      console.error(logObject);
    }
  }
}
