import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { DateConfig, DateService } from './date/';
export { DateService };

import { MediaConfig, MediaService } from './media';
export { MediaService };

import {
  AuthenticationConfig,
  AuthenticationService,
  AuthenticationInterceptor,
} from './authentication';
export { AuthenticationService, AuthenticationInterceptor };

import { AllorsFocusDirective, AllorsFocusService } from './focus';
export { AllorsFocusDirective, AllorsFocusService };
import { AllorsBarcodeDirective, AllorsBarcodeService } from './barcode';
export { AllorsBarcodeDirective,AllorsBarcodeService };

import {
  DatabaseService,
  WorkspaceService,
  ContextService,
  DatabaseConfig,
} from './framework';
export { DatabaseService, WorkspaceService, ContextService };

@NgModule({
  declarations: [AllorsBarcodeDirective, AllorsFocusDirective],
  exports: [AllorsBarcodeDirective, AllorsFocusDirective],
  imports: [CommonModule],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthenticationInterceptor,
      multi: true,
    },
  ],
})
export class AllorsAngularModule {
  static forRoot(config?: {
    databaseConfig: Partial<DatabaseConfig>;
    authenticationConfig: Partial<AuthenticationConfig>;
    dateConfig: Partial<DateConfig>;
    mediaConfig: Partial<MediaConfig>;
  }): ModuleWithProviders<AllorsAngularModule> {
    return {
      ngModule: AllorsAngularModule,
      providers: [
        DatabaseService,
        WorkspaceService,
        ContextService,
        { provide: DatabaseConfig, useValue: config?.databaseConfig },
        AuthenticationService,
        {
          provide: AuthenticationConfig,
          useValue: config?.authenticationConfig,
        },
        DateService,
        { provide: DateConfig, useValue: config?.dateConfig },
        MediaService,
        { provide: MediaConfig, useValue: config?.mediaConfig },
      ],
    };
  }
}
