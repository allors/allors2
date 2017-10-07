import { NgModule } from "@angular/core";
import { FlexLayoutModule } from "@angular/flex-layout";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpModule } from "@angular/http";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";

import { AuthenticationService, ENVIRONMENT } from "../allors/angular";
import { environment } from "../environments/environment";
import { AllorsService } from "./allors.service";
import { LoginComponent } from "./auth/login.component";

import { DashboardComponent } from "./dashboard/dashboard.component";
import { FormComponent } from "./form/form.component";

import { MATERIAL } from "../allors/material";

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

@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
    LoginComponent,
    DashboardComponent,
    FormComponent,
    MATERIAL,
  ],
  imports: [
    MATERIAL_MODULES,
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule,
    BrowserAnimationsModule,
    FlexLayoutModule,
    AppRoutingModule,
  ],
  providers: [
    { provide: ENVIRONMENT, useValue: environment },
    AllorsService,
    AuthenticationService,
  ],
})
export class AppModule { }
