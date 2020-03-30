export abstract class LoggingService {

  public abstract trace(logObject: any): void;
  public abstract debug(logObject: any): void;
  public abstract info(logObject: any): void;
  public abstract warn(logObject: any): void;
  public abstract error(logObject: any): void;
  public abstract fatal(logObject: any): void;

}
