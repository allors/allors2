import { InjectionToken } from "@angular/core";
export declare const ENVIRONMENT: InjectionToken<Environment>;
export declare abstract class Environment {
    production: boolean;
    url: string;
    authenticationUrl: string;
}
