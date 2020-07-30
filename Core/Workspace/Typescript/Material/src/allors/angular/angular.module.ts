import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { DateConfig, DateService } from './core/date';
import { MediaConfig, MediaService } from './core/media';
import { AuthenticationConfig, AuthenticationService, AuthenticationInterceptor } from './core/authentication';

import { AllorsFocusDirective } from './core/focus';
export { AllorsFocusDirective };
import { AllorsBarcodeDirective } from './core/barcode';
export { AllorsBarcodeDirective };

@NgModule({
  declarations: [AllorsBarcodeDirective, AllorsFocusDirective],
  exports: [AllorsBarcodeDirective, AllorsFocusDirective],
  imports: [CommonModule],
  providers: [{ provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true }],
})
export class AllorsAngularModule {
  static forRoot(config?: {
    dateConfig: Partial<DateConfig>;
    mediaConfig: Partial<MediaConfig>;
    authenticationConfig: Partial<AuthenticationConfig>;
  }): ModuleWithProviders<AllorsAngularModule> {
    return {
      ngModule: AllorsAngularModule,
      providers: [
        DateService,
        { provide: DateConfig, useValue: config?.dateConfig },
        MediaService,
        { provide: MediaConfig, useValue: config?.mediaConfig },
        AuthenticationService,
        { provide: AuthenticationConfig, useValue: config?.authenticationConfig },
      ],
    };
  }
}
