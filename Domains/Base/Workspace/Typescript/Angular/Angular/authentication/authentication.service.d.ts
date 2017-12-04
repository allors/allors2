import { HttpClient } from "@angular/common/http";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/map";
import { Environment } from "../core/Environment";
export declare class AuthenticationService {
    private http;
    private environment;
    private tokenName;
    token: string;
    constructor(http: HttpClient, environment: Environment);
    login$(userName: string, password: string): any;
}
