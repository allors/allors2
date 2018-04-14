import { Injectable } from "@angular/core";
import { JL } from "jsnlog";
import { LoggingService } from "../allors/angular";

import { environment } from "../environments/environment";

import { v4 as uuid } from "uuid";

function dump(o: any){
  var cache = [];
  var result = JSON.stringify(o, (key, value) => {
      if (typeof value === 'object' && value !== null) {
          if (cache.indexOf(value) !== -1) {
              return;
          }
          cache.push(value);
      }
      
      return value;
  });

  cache = null;
  return result;
}

@Injectable()
export class DefaultLoggingService extends LoggingService {
  private logger: JL.JSNLogLogger;

  constructor() {
    super();

    const requestId = uuid();

    JL.setOptions({
      defaultAjaxUrl: environment.url + "logging",
      requestId
    });

    const level = environment.production
      ? JL.getWarnLevel()
      : JL.getDebugLevel();
    this.logger = JL("Intranet").setOptions({ level });
  }

  public trace(logObject: any) {
    this.logger.trace(dump(logObject));
  }

  public debug(logObject: any) {
    this.logger.debug(dump(logObject));
  }

  public info(logObject: any) {
    this.logger.info(dump(logObject));
  }

  public warn(logObject: any) {
    this.logger.warn(dump(logObject));
  }

  public error(logObject: any) {
    var x = dump(logObject);
    this.logger.error(dump(logObject));
  }

  public fatal(logObject: any) {
    this.logger.fatal(dump(logObject));
  }
}
