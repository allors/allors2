import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { AuthenticationConfig } from './authentication.config';
import { AuthenticationTokenRequest } from './AuthenticationTokenRequest';
import { AuthenticationTokenResponse } from './AuthenticationTokenResponse';

@Injectable()
export class AuthenticationService {
  private tokenName = 'ALLORS_JWT';

  public get token(): string {
    return sessionStorage.getItem(this.tokenName);
  }

  public set token(value: string) {
    sessionStorage.setItem(this.tokenName, value);
  }

  constructor(
    private http: HttpClient,
    private authenticationConfig: AuthenticationConfig,
  ) {}

  public login$(userName: string, password: string): any {
    const url = this.authenticationConfig.url;
    const request: AuthenticationTokenRequest = { userName, password };

    return this.http
      .post<AuthenticationTokenResponse>(url, request)
      .map((result) => {
        if (result.authenticated) {
          this.token = result.token;
        }

        return result;
      })
      .catch((error: any) => {
        const errMsg = error.message
          ? error.message
          : error.status
            ? `${error.status} - ${error.statusText}`
            : 'Server error';
        return Observable.throw(errMsg);
      });
  }
}
