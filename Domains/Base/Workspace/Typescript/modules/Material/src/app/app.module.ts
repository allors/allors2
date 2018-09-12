import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { CdkAccordionModule } from '@angular/cdk/accordion';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { environment } from '../environments/environment';
import { AuthorizationService } from './auth/authorization.service';
import { LoginComponent } from './auth/login.component';
import { MainComponent } from './main/main.component';

import * as relations from '../allors/material/custom/relations';
import * as tests from '../allors/material/custom/tests';

const CDK_MODULES: any[] = [
  CdkAccordionModule
];

import {
  MatAutocompleteModule, MatButtonModule, MatCardModule, MatCheckboxModule, MatDatepickerModule,
  MatIconModule, MatInputModule, MatListModule, MatMenuModule,
  MatNativeDateModule, MatRadioModule, MatSelectModule,
  MatSidenavModule, MatSliderModule, MatSlideToggleModule,
  MatSnackBarModule, MatTabsModule, MatToolbarModule, MatTooltipModule, MAT_DATE_LOCALE, MAT_DATE_FORMATS, DateAdapter,
} from '@angular/material';

const MATERIAL_MODULES: any[] = [
  MatAutocompleteModule, MatButtonModule, MatCardModule, MatCheckboxModule, MatDatepickerModule,
  MatIconModule, MatInputModule, MatListModule, MatMenuModule,
  MatNativeDateModule, MatRadioModule, MatSelectModule,
  MatSidenavModule, MatSliderModule, MatSlideToggleModule,
  MatSnackBarModule, MatTabsModule, MatToolbarModule, MatTooltipModule,
];

import {
  AuthenticationConfig, AuthenticationInterceptor, AuthenticationService,
  DatabaseConfig, DatabaseService, WorkspaceService, ErrorService, LoggingService, MediaService, MenuService, DataService
} from '../allors/angular';

import {
  AllorsMaterialAutoCompleteModule, AllorsMaterialCheckboxModule, AllorsMaterialDatepickerModule, AllorsMaterialErrorDialogModule, AllorsMaterialFileModule,
  AllorsMaterialFilesModule, AllorsMaterialInputModule, AllorsMaterialLocalisedTextModule,
  AllorsMaterialRadioGroupModule, AllorsMaterialSelectModule, AllorsMaterialSideMenuModule, AllorsMaterialSliderModule, AllorsMaterialSlideToggleModule,
  AllorsMaterialStaticModule, AllorsMaterialTextAreaModule, AllorsMaterialDefaultErrorService, AllorsMaterialErrorDialogComponent,
  MomentUtcDateAdapter,
  AllorsMaterialSideNavService,
  AllorsMaterialDialogService,
  AllorsMaterialSideNavToggleModule,
} from '../allors/material';

import { MAT_MOMENT_DATE_FORMATS } from '@angular/material-moment-adapter';
import { DefaultLoggingService } from './app.logging.service';
import { DashboardComponent } from './dashboard/dashboard.component';

const BASE_MATERIAL_MODULES: any[] = [
  AllorsMaterialAutoCompleteModule, AllorsMaterialCheckboxModule, AllorsMaterialDatepickerModule, AllorsMaterialErrorDialogModule, AllorsMaterialInputModule,
  AllorsMaterialFileModule, AllorsMaterialFilesModule, AllorsMaterialLocalisedTextModule, AllorsMaterialRadioGroupModule, AllorsMaterialSelectModule,
  AllorsMaterialSideMenuModule, AllorsMaterialSideNavToggleModule,
  AllorsMaterialSliderModule, AllorsMaterialSlideToggleModule, AllorsMaterialStaticModule, AllorsMaterialTextAreaModule,
];

@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
    LoginComponent,
    MainComponent,
    DashboardComponent,
  ],
  imports: [
    CDK_MODULES,
    MATERIAL_MODULES,
    BASE_MATERIAL_MODULES,

    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    AppRoutingModule,

    relations.Modules,
    tests.Modules,
  ],
  providers: [
    { provide: DatabaseConfig, useValue: { url: environment.url } },
    { provide: AuthenticationConfig, useValue: { url: environment.url + environment.authenticationUrl } },
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true },
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS },
    { provide: DateAdapter, useClass: MomentUtcDateAdapter },
    { provide: LoggingService, useClass: DefaultLoggingService },
    { provide: ErrorService, useClass: AllorsMaterialDefaultErrorService },
    AllorsMaterialSideNavService,
    AllorsMaterialDialogService,
    MenuService,
    DatabaseService,
    WorkspaceService,
    DataService,
    AuthenticationService,
    AuthorizationService,
    MediaService
  ],
})
export class AppModule { }
