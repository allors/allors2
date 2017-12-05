import { HttpClient } from "@angular/common/http";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/map";
import { AuthenticationConfig } from "./authentication.config";
export declare class AuthenticationService {
    private http;
    private authenticationConfig;
    private tokenName;
    token: string;
    constructor(http: HttpClient, authenticationConfig: AuthenticationConfig);
    login$(userName: string, password: string): any;
}
