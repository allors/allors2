import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpModule } from "@angular/http";
import { BrowserModule, Title } from "@angular/platform-browser";

import { AuthenticationInterceptor, AuthenticationService, ENVIRONMENT } from "@allors/base-angular";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";

import { environment } from "../environments/environment";
import { AllorsService } from "./allors.service";
import { LoginComponent } from "./auth/login.component";

import { HttpClient } from "@angular/common/http/src/client";
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
    HttpModule,
    AppRoutingModule,
  ],
  providers: [
    { provide: ENVIRONMENT, useValue: environment },
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true },
    AllorsService,
    AuthenticationService,
    Title,
  ],
})
export class AppModule { }
