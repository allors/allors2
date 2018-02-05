import { Injectable } from "@angular/core";
import { JL } from "jsnlog";
import { LoggingService } from "../allors/angular";

import { environment } from "../environments/environment";

import { v4 as uuid } from "uuid";

@Injectable()
export class DefaultLoggingService extends LoggingService {

  private logger: JL.JSNLogLogger;

  constructor() {
    super();

    const requestId = uuid();

    JL.setOptions({
      defaultAjaxUrl: environment.url + "logging",
      requestId,
    });

    const level = environment.production ? JL.getWarnLevel() : JL.getDebugLevel();
    this.logger = JL("Intranet").setOptions({level});
  }

  public trace(logObject: any) {
    this.logger.trace(logObject);
  }

  public debug(logObject: any) {
    this.logger.debug(logObject);
  }

  public info(logObject: any) {
    this.logger.info(logObject);
  }

  public warn(logObject: any) {
    this.logger.warn(logObject);
  }

  public error(logObject: any) {
    this.logger.error(logObject);
  }

  public fatal(logObject: any) {
    this.logger.fatal(logObject);
  }
}
