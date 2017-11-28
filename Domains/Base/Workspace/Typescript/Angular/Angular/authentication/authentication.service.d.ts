import { HttpClient } from "@angular/common/http";
import { Environment } from "../core/Environment";
export declare class AuthenticationService {
    private http;
    private environment;
    token: string;
    constructor(http: HttpClient, environment: Environment);
    login$(userName: string, password: string): any;
}
