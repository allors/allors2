import { Observable } from 'rxjs';

export abstract class ErrorService {
  public abstract handle(error: Error): Observable<any>;
}
