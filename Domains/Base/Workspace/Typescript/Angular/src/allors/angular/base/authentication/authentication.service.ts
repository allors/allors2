import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";

import { ENVIRONMENT, Environment } from "@baseAngular/core/Environment";
import { AuthenticationTokenRequest } from "./AuthenticationTokenRequest";
import { AuthenticationTokenResponse } from "./AuthenticationTokenResponse";

@Injectable()
export class AuthenticationService {

  public token: string;

  constructor(private http: HttpClient, @Inject(ENVIRONMENT) private environment: Environment) { }

  public login$(userName: string, password: string): any {
    const url = this.environment.url + this.environment.authenticationUrl;
    const request: AuthenticationTokenRequest = { userName, password };

    return this.http
      .post<AuthenticationTokenResponse>(url, request)
      .map((result) => {
        if (result.authenticated) {
          this.token =  result.token;
        }

        return result;
      },
      ).catch((error: any) => {
        const errMsg = (error.message) ? error.message : error.status ? `${error.status} - ${error.statusText}` : "Server error";
        return Observable.throw(errMsg);
      });
  }
}
