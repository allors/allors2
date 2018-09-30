import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule, Title } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CdkAccordionModule } from '@angular/cdk/accordion';
import { MAT_MOMENT_DATE_FORMATS } from '@angular/material-moment-adapter';

import { AuthorizationService } from './auth/authorization.service';
import { LoginComponent } from './auth/login.component';
import { MainComponent } from './main/main.component';
import { DefaultLoggingService } from './app.logging.service';
import { DashboardComponent } from './dashboard/dashboard.component';
import { environment } from '../environments/environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ConfigService } from './app.config.service';

import { DefaultStateService } from '../allors/material/apps/services/DefaultStateService';
import { StateService } from '../allors/material/apps/services/StateService';
import { AllorsMaterialFilterModule } from '../allors/material/base/components/filter';

import * as ap from '../allors/material/apps/components/accountspayable';
import * as ar from '../allors/material/apps/components/accountsreceivable';
import * as catalogues from '../allors/material/apps/components/catalogues';
import * as orders from '../allors/material/apps/components/orders';
import * as relations from '../allors/material/apps/components/relations';
import * as workefforts from '../allors/material/apps/components/workefforts';

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
  DatabaseConfig, DatabaseService, WorkspaceService, ErrorService, LoggingService, MediaService, MenuService, PdfService, Allors, AllorsFocusModule, AllorsModule
} from '../allors/angular';

import {
  AllorsMaterialAutoCompleteModule, AllorsMaterialCheckboxModule, AllorsMaterialDatepickerModule, AllorsMaterialDatetimepickerModule, AllorsMaterialDialogModule, AllorsMaterialErrorDialogModule, AllorsMaterialFileModule, AllorsMaterialFilesModule, AllorsMaterialInputModule, AllorsMaterialLocalisedTextModule,
  AllorsMaterialRadioGroupModule, AllorsMaterialSelectModule, AllorsMaterialSideMenuModule, AllorsMaterialSideNavToggleModule, AllorsMaterialSliderModule, AllorsMaterialSlideToggleModule,
  AllorsMaterialStaticModule, AllorsMaterialTextAreaModule, MomentUtcDateAdapter, AllorsMaterialDefaultErrorService, AllorsMaterialErrorDialogComponent, AllorsMaterialSideNavService, AllorsMaterialDialogService
} from '../allors/material';

const ALLORS_MATERIAL_MODULES: any[] = [
  AllorsMaterialAutoCompleteModule, AllorsMaterialCheckboxModule, AllorsMaterialDatepickerModule, AllorsMaterialDatetimepickerModule, AllorsMaterialDialogModule, AllorsMaterialErrorDialogModule, AllorsMaterialInputModule, AllorsMaterialFileModule, AllorsMaterialFilesModule, AllorsMaterialLocalisedTextModule,
  AllorsMaterialRadioGroupModule, AllorsMaterialSelectModule, AllorsMaterialSideMenuModule, AllorsMaterialSideNavToggleModule, AllorsMaterialSliderModule, AllorsMaterialSlideToggleModule, AllorsMaterialStaticModule, AllorsMaterialTextAreaModule,
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
    ALLORS_MATERIAL_MODULES,
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AllorsModule.forRoot(),
    AllorsFocusModule.forRoot(),
    AllorsMaterialFilterModule.forRoot(),

    relations.Modules,
    orders.Modules,
    catalogues.Modules,
    ap.modules,
    ar.modules,
    workefforts.Modules,
  ],
  providers: [
    Title,
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true },
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS },
    { provide: DateAdapter, useClass: MomentUtcDateAdapter },
    { provide: DatabaseConfig, useValue: { url: environment.url } },
    { provide: AuthenticationConfig, useValue: { url: environment.url + environment.authenticationUrl } },
    { provide: StateService, useClass: DefaultStateService },
    { provide: LoggingService, useClass: DefaultLoggingService },
    { provide: ErrorService, useClass: AllorsMaterialDefaultErrorService },
    AllorsMaterialSideNavService,
    AllorsMaterialDialogService,
    AuthenticationService,
    AuthorizationService,
    ConfigService,
    MediaService,
    MenuService,
    PdfService,
  ],
})
export class AppModule { }
