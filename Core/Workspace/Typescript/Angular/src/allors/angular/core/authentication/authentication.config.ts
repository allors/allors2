import { Injectable } from '@angular/core';

@Injectable()
export class AuthenticationConfig {
  constructor(public url: string) {}
}
