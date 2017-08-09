import { Injectable, Inject , ChangeDetectorRef } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';

import { Router, CanActivate } from '@angular/router';

import { Observable } from 'rxjs/Observable';

import { ENVIRONMENT, Environment } from '../Environment';

@Injectable()
export class AuthenticationService implements CanActivate {
  private tokenKey = 'token';

  constructor(private http: Http, private router: Router, @Inject(ENVIRONMENT) private environment: Environment) { }

  public canActivate() {
    if (this.checkLogin()) {
      return true;
    } else {
      this.router.navigate(['login']);
      return false;
    }
  }

  public postProcessRequestOptions(requestOptions: RequestOptions): RequestOptions {
    const postProcessedRequestOptions = new RequestOptions(requestOptions);

    const token = this.getLocalToken();
    postProcessedRequestOptions.headers.set('Authorization', 'Bearer ' + token);

    return postProcessedRequestOptions;
  }

  public login$(userName: string, password: string) {
    const header = new Headers({ 'Content-Type': 'application/json' });
    const body = JSON.stringify({ 'Username': userName, 'Password': password });
    const options = new RequestOptions({ headers: header });
    const url = this.environment.url + this.environment.authenticationUrl;

    return this.http
      .post(url, body, options)
      .map(
      res => {
        const result = res.json();
        if (result.authenticated) {
          sessionStorage.setItem(this.tokenKey, result.token);
        }

        return result;
      }
      ).catch((error: any) => {
        const errMsg = (error.message) ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error';
        console.error(errMsg);
        return Observable.throw(errMsg);
      });
  }

  public checkLogin(): boolean {
    const token = sessionStorage.getItem(this.tokenKey);
    return token != null;
  }

  private getLocalToken(): string {
    return sessionStorage.getItem(this.tokenKey);
  }

  private initAuthHeaders(): Headers {
    const token = this.getLocalToken();
    if (token == null) { throw new Error('No token'); };

    const headers = new Headers({ 'Content-Type': 'application/json' });
    headers.append('Authorization', 'Bearer ' + token);
    return headers;
  }
}
