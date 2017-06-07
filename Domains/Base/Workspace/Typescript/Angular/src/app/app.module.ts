import { BrowserModule, Title } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { ENVIRONMENT, AuthenticationService } from '../allors/angular';
import { environment } from '../environments/environment';
import { AllorsService } from './allors.service';
import { LoginComponent } from './auth/login.component';

import { HomeComponent } from './home/home.component';
import { FormComponent } from './form/form.component';
import { QueryComponent } from './query/query.component';
import { FetchComponent } from './fetch/fetch.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    FormComponent,
    QueryComponent,
    FetchComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule,
    AppRoutingModule
  ],
  providers: [
    { provide: ENVIRONMENT, useValue: environment },
    AllorsService,
    AuthenticationService,
    Title
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
