import { InjectionToken  } from "@angular/core";

export const ENVIRONMENT: InjectionToken<Environment> = new InjectionToken ("environment");

// Use abstract class instead of interface.
// See https://github.com/angular/angular-cli/issues/2034#issuecomment-302666897
export abstract class Environment {
    public production: boolean;
    public url: string;
    public authenticationUrl: string;
}
