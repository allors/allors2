import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import {
  DateConfig,
  DateService,
  MediaConfig,
  MediaService,
  AuthenticationConfig,
  AuthenticationService,
  AuthenticationInterceptor,
  AllorsFocusDirective,
  AllorsFocusService,
  AllorsBarcodeDirective,
  AllorsBarcodeService,
  DatabaseService,
  WorkspaceService,
  ContextService,
  DatabaseConfig,
} from '@allors/angular/core';

import {
  FetcherService, InternalOrganisationId,
} from '@allors/angular/base';

export {
  // Core
  DateService,
  MediaService,
  AuthenticationService,
  AuthenticationInterceptor,
  AllorsFocusDirective,
  AllorsFocusService,
  AllorsBarcodeDirective,
  AllorsBarcodeService,
  DatabaseService,
  WorkspaceService,
  ContextService,
  // Base
  FetcherService,
  InternalOrganisationId,
};

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
        { provide: DatabaseConfig, useValue: config?.databaseConfig },
        WorkspaceService,
        ContextService,
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
