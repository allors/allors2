import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { AuthenticationTokenResponse } from './AuthenticationTokenResponse';

@Injectable()
export abstract class AuthenticationService {
  abstract readonly token: string | null;

  abstract login$(userName: string, password: string): Observable<AuthenticationTokenResponse>;
  
  abstract logout(): void;
}
