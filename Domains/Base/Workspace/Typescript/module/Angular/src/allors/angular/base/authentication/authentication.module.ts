import { NgModule, ModuleWithProviders } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { AuthenticationService } from './authentication.service';
import { AuthenticationInterceptor } from './authentication.interceptor';
import { AuthenticationConfig } from './authentication.config';

export { AuthenticationService } from './authentication.service';

@NgModule({
  declarations: [
  ],
  exports: [
  ],
  imports: [
  ],
  providers: [
    AuthenticationService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true },
  ]
})
export class AuthenticationModule {
  static forRoot(config: AuthenticationConfig): ModuleWithProviders {
    return {
      ngModule: AuthenticationModule,
      providers: [
        AuthenticationService,
        { provide: AuthenticationConfig, useValue: config },
      ]
    };
  }
}
