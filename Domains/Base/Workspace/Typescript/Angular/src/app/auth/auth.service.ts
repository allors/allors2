import { Injectable } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';

import { Router, CanActivate } from '@angular/router';

import { Observable } from 'rxjs/Observable';

import { AllorsService } from '../allors.service';

@Injectable()
export class AuthService implements CanActivate {
    private tokeyKey = 'token';

    constructor(private http: Http, private router: Router, private allorsService: AllorsService) { }

    public canActivate() {
        if (this.checkLogin()) {
            return true;
        } else {
            this.router.navigate(['login']);
            return false;
        }
    }

    public login$(userName: string, password: string) {
        const header = new Headers({ 'Content-Type': 'application/json' });
        const body = JSON.stringify({ 'Username': userName, 'Password': password });
        const options = new RequestOptions({ headers: header });
        const url = this.allorsService.url + 'Authentication/SignIn';

        return this.http.post(url, body, options)
        .map(
            res => {
                const result = res.json();
                if (result.state === 1 && result.data && result.data.accessToken) {
                    sessionStorage.setItem(this.tokeyKey, result.data.accessToken);
                }
                return result;
            }
        ).catch(this.handleError);
    }

    public authGet$(url) {
        const headers = this.initAuthHeaders();
        const options = new RequestOptions({ headers: headers });
        return this.http.get(url, options).map(
            response => response.json()
        ).catch(this.handleError);
    }

    public checkLogin(): boolean {
        const token = sessionStorage.getItem(this.tokeyKey);
        return token != null;
    }

    private getLocalToken(): string {
           return sessionStorage.getItem(this.tokeyKey);
    }

    private initAuthHeaders(): Headers {
        const token = this.getLocalToken();
        if (token == null) {throw new Error('No token')};

        const headers = new Headers({ 'Content-Type': 'application/json' });
        headers.append('Authorization', 'Bearer ' + token);
        return headers;
    }

    private handleError(error: any) {
        const errMsg = (error.message) ? error.message :
            error.status ? `${error.status} - ${error.statusText}` : 'Server error';
        console.error(errMsg);
        return Observable.throw(errMsg);
    }
}
