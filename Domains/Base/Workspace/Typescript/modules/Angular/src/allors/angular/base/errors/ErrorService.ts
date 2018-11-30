import { Observable } from 'rxjs';
import { ResponseError } from 'src/allors/framework';

export abstract class ErrorService {

  public abstract handler: (error) => void;

  public abstract handle(error: Error): Observable<any>;

}
