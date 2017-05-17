import { Injectable } from '@angular/core';
import { Observable, Observer } from 'rxjs/Rx';

import { Http, Response, Headers, RequestOptions } from '@angular/http';

import { AllorsService } from '../allors.service';
import { IUser } from './user';

@Injectable()
export class AuthService {

    isLoggedIn: boolean;
    redirectUrl: string;


    constructor(private http: Http, private allorsService: AllorsService) {
        this.logout();
    }

    checkLoggedIn(): Promise<boolean> {
        return new Promise<boolean>( (resolve, reject) => {
            if (this.isLoggedIn) {
                resolve(this.isLoggedIn);
            } else {
                this.refresh()
                    .then(v => {
                        resolve(this.isLoggedIn);
                    })
                    .catch(v => {
                        resolve(this.isLoggedIn);
                    });
            };
        });
    }

    login(userName: string, password: string): Promise<boolean> {
        const url = this.allorsService.url + 'Authentication/SignIn';
        const body = {
            userName: userName,
            password: password,
            isPersistent: true
        };
        const headers = new Headers(
            {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            });
        const options = new RequestOptions(
            {
                headers: headers,
                withCredentials: true
             });

        return this.http.post(url, body, options)
            .toPromise()
            .then(v => {
                this.isLoggedIn = true;
                return true;
            })
            .catch(v => {
                return false;
            });
    }

    logout(): void {
        // TODO: Call logout on server
        this.isLoggedIn = false;
    }

    private refresh(): Promise<boolean> {
        const url = this.fullyQualifiedUrl('Authentication/Refresh');
        const options = new RequestOptions(
            {
                withCredentials: true
             });

        return this.http.post(url, null, options)
            .toPromise()
            .then(v => {
                this.isLoggedIn = true;
                return true;
            })
            .catch(v => {
                return false;
            });
    }

     private fullyQualifiedUrl(localUrl: string): string {
        return this.allorsService.url + localUrl;
    }
}
