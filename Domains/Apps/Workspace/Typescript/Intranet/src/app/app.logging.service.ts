import { Injectable } from '@angular/core';
import { LoggingService } from '../allors/angular';

@Injectable()
export class DefaultLoggingService extends LoggingService {

  constructor() {
    super();
  }

  public trace(logObject: any) {
    // tslint:disable-next-line:no-console
    console.trace(logObject);
  }

  public debug(logObject: any) {
    // tslint:disable-next-line:no-console
    console.debug(logObject);
  }

  public info(logObject: any) {
    // tslint:disable-next-line:no-console
    console.info(logObject);
  }

  public warn(logObject: any) {
    console.warn(logObject);
  }

  public error(logObject: any) {
    console.error(logObject);
  }

  public fatal(logObject: any) {
    console.error(logObject);
  }
}
