import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LoggingService {
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
