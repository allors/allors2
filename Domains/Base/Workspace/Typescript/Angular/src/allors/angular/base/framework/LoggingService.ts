export abstract class LoggingService {

  public abstract trace(logObject: any);
  public abstract debug(logObject: any);
  public abstract info(logObject: any);
  public abstract warn(logObject: any);
  public abstract error(logObject: any);
  public abstract fatal(logObject: any);

}
