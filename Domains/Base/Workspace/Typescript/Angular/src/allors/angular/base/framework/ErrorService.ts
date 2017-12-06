export abstract class ErrorService {
  public abstract message(error: Error): void;
  public abstract dialog(error: Error): any;
}
