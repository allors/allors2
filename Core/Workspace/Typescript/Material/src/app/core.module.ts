import { HttpClientModule } from '@angular/common/http';
import { NgModule, Optional, SkipSelf } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';

import { enGB } from 'date-fns/locale';

import { environment } from '../environments/environment';

import {
  AllorsModule,
  AllorsFocusModule,
  AllorsBarcodeModule,
  AllorsFilterModule,
  AllorsRefreshModule,
  AuthenticationModule,
  MediaModule,
  NavigationModule,
  DateModule,
} from '../allors/angular';

import { DeleteModule, NavigateModule, DialogModule, LoggingModule, SideNavModule, AllorsDateAdapter } from '../allors/material';
import { SaveModule } from '../allors/material/core/services/save';
import { MAT_AUTOCOMPLETE_DEFAULT_OPTIONS } from '@angular/material/autocomplete';

@NgModule({
  imports: [
    BrowserModule,
    environment.production ? BrowserAnimationsModule : NoopAnimationsModule,
    RouterModule,
    HttpClientModule,

    AllorsModule.forRoot({ url: environment.url }),
    AuthenticationModule.forRoot({
      url: environment.url + environment.authenticationUrl,
    }),
    LoggingModule.forRoot({ console: true }),
    SaveModule,

    AllorsBarcodeModule.forRoot(),
    AllorsFocusModule.forRoot(),
    AllorsFilterModule.forRoot(),
    AllorsRefreshModule.forRoot(),
    DialogModule.forRoot(),
    MediaModule.forRoot({ url: environment.url }),

    // Menu/Navigation
    NavigationModule.forRoot(),
    SideNavModule.forRoot(),

    // Actions
    DeleteModule.forRoot(),
    NavigateModule.forRoot(),

    // Date
    DateModule.forRoot({
      locale: enGB,
    }),
  ],
  providers: [
    {
      provide: MAT_AUTOCOMPLETE_DEFAULT_OPTIONS,
      useValue: { autoActiveFirstOption: true },
    },
    { provide: DateAdapter, useClass: AllorsDateAdapter },
    {
      provide: MAT_DATE_FORMATS,
      useValue: {
        parse: {
          dateInput: 'dd/MM/yyyy',
        },
        display: {
          dateInput: 'dd/MM/yyyy',
          monthYearLabel: 'LLL y',
          dateA11yLabel: 'MMMM d, y',
          monthYearA11yLabel: 'MMMM y',
        },
      },
    },
  ],
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() core: CoreModule) {
    if (core) {
      throw new Error('Only use CoreModule from AppModule');
    }
  }
}
