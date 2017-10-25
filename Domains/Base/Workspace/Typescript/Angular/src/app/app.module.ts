import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { BrowserModule, Title } from "@angular/platform-browser";

import { AllorsService, AuthenticationInterceptor, AuthenticationService, ENVIRONMENT } from "@allors";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";

import { environment } from "../environments/environment";
import { DefaultAllorsService } from "./allors.service";
import { LoginComponent } from "./auth/login.component";

import { FetchComponent } from "./fetch/fetch.component";
import { FormComponent } from "./form/form.component";
import { HomeComponent } from "./home/home.component";
import { QueryComponent } from "./query/query.component";

@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    FormComponent,
    QueryComponent,
    FetchComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
  ],
  providers: [
    { provide: ENVIRONMENT, useValue: environment },
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true },
    { provide: AllorsService, useClass: DefaultAllorsService },
    AuthenticationService,
    Title,
  ],
})
export class AppModule { }
