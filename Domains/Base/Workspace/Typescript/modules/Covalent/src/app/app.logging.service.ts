import { Injectable } from "@angular/core";
import { LoggingService } from "../allors/angular";
import { environment } from "../environments/environment";

@Injectable()
export class DefaultLoggingService extends LoggingService {

  public trace(logObject: any) {
    console.trace(logObject);
  }

  public debug(logObject: any) {
    console.debug(logObject);
  }

  public info(logObject: any) {
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
