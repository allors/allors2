import { ErrorHandler, Injectable } from "@angular/core";
import { LoggingService } from "../allors/angular";

@Injectable()
export class DefaultErrorHandler implements ErrorHandler {

  constructor(private loggingService: LoggingService) {
  }

  public handleError(error: any) {
    this.loggingService.fatal(error);
  }
}
