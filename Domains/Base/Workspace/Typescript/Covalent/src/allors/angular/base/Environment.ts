import { OpaqueToken } from '@angular/core';

export let ENVIRONMENT = new OpaqueToken('environment');

// Use abstract class instead of interface.
// See https://github.com/angular/angular-cli/issues/2034#issuecomment-302666897
export abstract class Environment {
    production: boolean;
    url: string;
    authenticationUrl: string;
}
