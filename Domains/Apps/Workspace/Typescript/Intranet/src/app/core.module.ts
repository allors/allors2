import { HttpClientModule } from '@angular/common/http';
import { NgModule, Optional, SkipSelf, ErrorHandler } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { MAT_DATE_LOCALE, MAT_DATE_FORMATS, DateAdapter } from '@angular/material';
import { MAT_MOMENT_DATE_FORMATS } from '@angular/material-moment-adapter';

import { environment } from '../environments/environment';

import { AllorsModule, AllorsFocusModule, AllorsBarcodeModule, AllorsFilterModule, AllorsRefreshModule, AuthenticationModule, MediaModule, NavigationModule } from '../allors/angular';
import { MomentUtcDateAdapter, DeleteModule, NavigateModule, DialogModule, LoggingModule, SideNavModule, StateService, MethodModule, PrintModule } from '../allors/material';

import { DefaultStateService } from '../allors/material/apps/services/state/default.state.service';
import { ConfigService } from './app.config.service';
import { ErrorModule } from './error/error.module';

@NgModule({
  imports: [
    BrowserModule,
    environment.production ? BrowserAnimationsModule : NoopAnimationsModule,
    RouterModule,
    HttpClientModule,
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
    NavigateModule.forRoot()
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS },
    { provide: DateAdapter, useClass: MomentUtcDateAdapter },
    { provide: StateService, useClass: DefaultStateService },
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
