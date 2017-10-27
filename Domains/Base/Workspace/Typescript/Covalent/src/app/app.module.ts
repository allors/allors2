import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { NgModule, Type } from "@angular/core";
import { BrowserModule, Title } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

import { ErrorService } from "@baseAngular";
import { DefaultErrorService } from "@baseCovalent";

import { AppRoutingModule, routedComponents } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { SharedModule } from "./shared/shared.module";

import { AllorsService, AuthenticationInterceptor, AuthenticationService, ENVIRONMENT } from "@baseAngular";
import { environment } from "../environments/environment";

import { DefaultAllorsService } from "./allors.service";

import { COVALENT } from "@baseCovalent";
import { MATERIAL } from "@baseMaterial";

import { RELATIONS } from "../allors/covalent/custom/relations";

import {
  MatAutocompleteModule, MatButtonModule, MatCardModule, MatCheckboxModule, MatDatepickerModule,
  MatIconModule, MatInputModule, MatListModule, MatMenuModule,
  MatNativeDateModule, MatRadioModule, MatSelectModule,
  MatSidenavModule, MatSliderModule, MatSlideToggleModule,
  MatSnackBarModule, MatTabsModule, MatToolbarModule, MatTooltipModule,
} from "@angular/material";

const MATERIAL_MODULES: any[] = [
  MatButtonModule, MatCardModule, MatDatepickerModule, MatIconModule, MatAutocompleteModule,
  MatListModule, MatMenuModule, MatTooltipModule,
  MatSlideToggleModule, MatInputModule, MatCheckboxModule,
  MatToolbarModule, MatSnackBarModule, MatSidenavModule,
  MatTabsModule, MatSelectModule, MatRadioModule, MatSliderModule,
];

import {
  CovalentChipsModule, CovalentCommonModule, CovalentDataTableModule,
  CovalentDialogsModule, CovalentFileModule, CovalentLayoutModule,
  CovalentLoadingModule, CovalentMediaModule, CovalentMenuModule,
  CovalentNotificationsModule, CovalentPagingModule, CovalentSearchModule, CovalentStepsModule,
} from "@covalent/core";

const COVALENT_MODULES: any[] = [
  CovalentChipsModule, CovalentDataTableModule, CovalentMediaModule, CovalentLoadingModule,
  CovalentNotificationsModule, CovalentLayoutModule, CovalentMenuModule,
  CovalentPagingModule, CovalentSearchModule, CovalentStepsModule,
  CovalentCommonModule, CovalentDialogsModule, CovalentFileModule, CovalentChipsModule,
];

@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    MATERIAL,
    COVALENT,
    RELATIONS,
    AppComponent,
    routedComponents,
  ],
  imports: [
    MATERIAL_MODULES,
    COVALENT_MODULES,
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    SharedModule,
  ],
  providers: [
    { provide: ENVIRONMENT, useValue: environment },
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true },
    { provide: ErrorService, useClass: DefaultErrorService },
    { provide: AllorsService, useClass: DefaultAllorsService },
    AuthenticationService,
    Title,
  ],
})
export class AppModule { }
