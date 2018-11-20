import { HttpClientModule } from '@angular/common/http';
import { NgModule, Optional, SkipSelf } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { MAT_DATE_LOCALE, MAT_DATE_FORMATS, DateAdapter } from '@angular/material';
import { MAT_MOMENT_DATE_FORMATS } from '@angular/material-moment-adapter';

import { environment } from '../environments/environment';
import { DefaultLoggingService } from './app.logging.service';

import {
  ErrorService, LoggingService, MediaService, MenuService, AllorsModule, AllorsFocusModule, NavigationService, AllorsBarcodeModule, AllorsFilterModule, AllorsRefreshModule, AuthenticationModule
} from '../allors/angular';

import {
  MomentUtcDateAdapter, AllorsMaterialSideNavService, DeleteModule, AllorsMaterialDialogService, AllorsMaterialDefaultErrorService,
} from '../allors/material';


@NgModule({
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    RouterModule,
    HttpClientModule,

    AllorsModule.forRoot({ url: environment.url }),
    AuthenticationModule.forRoot({ url: environment.url + environment.authenticationUrl }),

    AllorsBarcodeModule.forRoot(),
    AllorsFocusModule.forRoot(),
    AllorsFilterModule.forRoot(),
    AllorsRefreshModule.forRoot(),

    // Actions
    DeleteModule.forRoot(),
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS },
    { provide: DateAdapter, useClass: MomentUtcDateAdapter },
    { provide: LoggingService, useClass: DefaultLoggingService },
    { provide: ErrorService, useClass: AllorsMaterialDefaultErrorService },
    AllorsMaterialSideNavService,
    AllorsMaterialDialogService,
    NavigationService,
    MenuService,
    MediaService
  ],
})
export class CoreModule {

  constructor(@Optional() @SkipSelf() core: CoreModule) {
    if (core) {
      throw new Error('Only use CoreModule from AppModule');
    }
  }

}
