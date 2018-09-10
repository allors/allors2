import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { BrowserModule, Title } from '@angular/platform-browser';

import { AuthenticationConfig, AuthenticationInterceptor, AuthenticationService, DatabaseConfig, DatabaseService, WorkspaceService } from '../allors/angular';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { environment } from '../environments/environment';

import { AuthorizationService } from './auth/authorization.service';
import { LoginComponent } from './auth/login.component';

import { FetchComponent } from './fetch/fetch.component';
import { FormComponent } from './form/form.component';
import { HomeComponent } from './home/home.component';
import { QueryComponent } from './query/query.component';

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
    { provide: DatabaseConfig, useValue: { url: environment.url } },
    { provide: AuthenticationConfig, useValue: { url: environment.url + environment.authenticationUrl} },
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true },
    DatabaseService,
    WorkspaceService,
    AuthenticationService,
    AuthorizationService,
    Title,
  ],
})
export class AppModule { }
