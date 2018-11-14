import { Observable } from 'rxjs';

export abstract class ErrorService {

  public abstract handler: (error) => void;

  public abstract handle(error: Error): Observable<any>;
}
