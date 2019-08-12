import { HttpClientModule } from '@angular/common/http';
import { NgModule, Optional, SkipSelf, ErrorHandler } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { MAT_DATE_LOCALE } from '@angular/material/core';

import { environment } from '../environments/environment';

import { AllorsModule, AllorsFocusModule, AllorsBarcodeModule, AllorsFilterModule, AllorsRefreshModule, AuthenticationModule, MediaModule, NavigationModule } from '../allors/angular';
import { DeleteModule, NavigateModule, DialogModule, LoggingModule, SideNavModule, MethodModule, PrintModule, SaveModule, FiltersService } from '../allors/material';

import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';

import { ConfigService } from './app.config.service';
import { ErrorModule } from './error/error.module';

import { DefaultFiltersService } from '../allors/material/base/services/filters/default.filters.service';

@NgModule({
  imports: [
    BrowserModule,
    environment.production ? BrowserAnimationsModule : NoopAnimationsModule,
    RouterModule,
    HttpClientModule,
    SaveModule,
    ErrorModule,

    AllorsModule.forRoot({ url: environment.url }),
    AuthenticationModule.forRoot({
      url: environment.url + environment.authenticationUrl
    }),
    LoggingModule.forRoot({ console: true }),

    AllorsBarcodeModule.forRoot(),
    AllorsFocusModule.forRoot(),
    AllorsFilterModule.forRoot(),
    AllorsRefreshModule.forRoot(),
    DialogModule.forRoot(),
    MediaModule.forRoot({ url: environment.url }),
    PrintModule.forRoot({ url: environment.url }),

    // Menu/Navigation
    NavigationModule.forRoot(),
    SideNavModule.forRoot(),

    // Actions
    DeleteModule.forRoot(),
    MethodModule.forRoot(),
    NavigateModule.forRoot(),

    // Angular Calendar
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory
    })
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: FiltersService, useClass: DefaultFiltersService },
    ConfigService
  ]
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() core: CoreModule) {
    if (core) {
      throw new Error('Use CoreModule from AppModule');
    }
  }
}
