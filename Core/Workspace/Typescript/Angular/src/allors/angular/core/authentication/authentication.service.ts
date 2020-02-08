import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { map, catchError } from 'rxjs/operators';

import { AuthenticationConfig } from './authentication.config';
import { AuthenticationTokenRequest } from './AuthenticationTokenRequest';
import { AuthenticationTokenResponse } from './AuthenticationTokenResponse';
import { throwError, Observable } from 'rxjs';

import { UserId } from '../state/UserId';

@Injectable()
export class AuthenticationService {

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
  ) { }

  public login$(userName: string, password: string): Observable<AuthenticationTokenResponse> {
    const url = this.authenticationConfig.url;
    const request: AuthenticationTokenRequest = { userName, password };

    return this.http
      .post<AuthenticationTokenResponse>(url, request)
      .pipe(
        map((result) => {
          if (result.authenticated) {
            this.token = result.token;
            this.userIdState.value = result.userId;
          }

          return result;
        }),
        catchError((error: any) => {
          const errMsg = error.message
            ? error.message
            : error.status
              ? `${error.status} - ${error.statusText}`
              : 'Server error';
          return throwError(errMsg);
        })
      );
  }
}
