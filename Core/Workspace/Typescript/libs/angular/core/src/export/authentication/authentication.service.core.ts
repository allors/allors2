import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { map, catchError } from 'rxjs/operators';
import { throwError, Observable } from 'rxjs';

import { AuthenticationService, AuthenticationTokenResponse, UserId } from '@allors/angular/services/core';

import { AuthenticationConfig } from './authentication.config';
import { AuthenticationTokenRequest } from './AuthenticationTokenRequest';

@Injectable()
export class AuthenticationServiceCore extends AuthenticationService {
  private tokenName = 'ALLORS_JWT';

  public get token(): string | null {
    return sessionStorage.getItem(this.tokenName);
  }

  public set token(value: string | null) {
    if (value == null) {
      sessionStorage.removeItem(this.tokenName);
    } else {
      sessionStorage.setItem(this.tokenName, value);
    }
  }

  constructor(
    private http: HttpClient,
    private authenticationConfig: AuthenticationConfig,
    private userIdState: UserId,
    private router: Router
  ) {
    super();
  }

  login$(userName: string, password: string): Observable<AuthenticationTokenResponse> {
    const url = this.authenticationConfig.url;
    const request: AuthenticationTokenRequest = { userName, password };

    return this.http.post<AuthenticationTokenResponse>(url, request).pipe(
      map((result) => {
        if (result.authenticated) {
          this.token = result.token;
          this.userIdState.value = result.userId;
        }

        return result;
      }),
      catchError((error: any) => {
        const errMsg = error.message ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error';
        return throwError(errMsg);
      })
    );
  }

  logout() {
    this.token = null;
    this.router.navigate(['/login']);
  }
}
