import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";

import { environment } from "../environments/environment";
import { DefaultAllorsService } from "./allors.service";
import { LoginComponent } from "./auth/login.component";

import { DashboardComponent } from "./dashboard/dashboard.component";
import { FormComponent } from "./form/form.component";

import { SharedModule } from "./shared/shared.module";

import { AllorsService, AuthenticationInterceptor, AuthenticationService, ENVIRONMENT } from "@allors";

import { AutoCompleteModule, CheckboxModule, DatepickerModule, InputModule, LocalisedTextModule,
         RadioGroupModule, SelectModule, SliderModule, SlideToggleModule, StaticModule, TextAreaModule } from "@baseMaterial/index";

const BASE_MATERIAL_MODULES: any[] = [
  AutoCompleteModule, CheckboxModule, DatepickerModule, InputModule, LocalisedTextModule,
  RadioGroupModule, SelectModule, SliderModule, SlideToggleModule, StaticModule, TextAreaModule,
];

@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
    LoginComponent,
    DashboardComponent,
    FormComponent,
  ],
  imports: [
    BASE_MATERIAL_MODULES,
    SharedModule,
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    AppRoutingModule,
  ],
  providers: [
    { provide: ENVIRONMENT, useValue: environment },
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true },
    { provide: AllorsService, useClass: DefaultAllorsService },
    AuthenticationService,
  ],
})
export class AppModule { }
