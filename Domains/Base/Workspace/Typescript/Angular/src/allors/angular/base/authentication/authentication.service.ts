import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { Observable } from "rxjs/Observable";
import { ENVIRONMENT, Environment } from "../Environment";
import { AuthenticationTokenRequest } from "./AuthenticationTokenRequest";
import { AuthenticationTokenResponse } from "./AuthenticationTokenResponse";

@Injectable()
export class AuthenticationService implements CanActivate {
  private tokenKey = "token";

  constructor(private http: HttpClient, private router: Router, @Inject(ENVIRONMENT) private environment: Environment) { }

  public get token(): string {
    return sessionStorage.getItem(this.tokenKey);
  }

  public set token(value: string)
  {
    sessionStorage.setItem(this.tokenKey, value);
  }

  public canActivate() {
    if (this.token) {
      return true;
    } else {
      this.router.navigate(["login"]);
      return false;
    }
  }

  public login$(userName: string, password: string) {
    const url = this.environment.url + this.environment.authenticationUrl;
    const request: AuthenticationTokenRequest = { userName, password };

    return this.http
      .post<AuthenticationTokenResponse>(url, request)
      .map((result) => {
        if (result.authenticated) {
          sessionStorage.setItem(this.tokenKey, result.token);
        }

        return result;
      },
      ).catch((error: any) => {
        const errMsg = (error.message) ? error.message : error.status ? `${error.status} - ${error.statusText}` : "Server error";
        return Observable.throw(errMsg);
      });
  }
}
