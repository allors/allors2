import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MaterialModule, MdSnackBarModule, MdNativeDateModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { ENVIRONMENT, AuthenticationService } from '../allors/angular';
import { environment } from '../environments/environment';
import { AllorsService } from './allors.service';
import { LoginComponent } from './auth/login.component';

import { DashboardComponent } from './dashboard/dashboard.component';
import { FormComponent } from './form/form.component';

import * as angularMaterial from '../allors/angularMaterial';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    DashboardComponent,
    FormComponent,
    angularMaterial.DatepickerComponent,
    angularMaterial.EnumComponent,
    angularMaterial.RadioButtonComponent,
    angularMaterial.SelectComponent,
    angularMaterial.StaticComponent,
    angularMaterial.TextareaComponent,
    angularMaterial.TextComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule,
    MaterialModule,
    MdSnackBarModule,
    MdNativeDateModule,
    BrowserAnimationsModule,
    FlexLayoutModule,
    AppRoutingModule
  ],
  providers: [
    { provide: ENVIRONMENT, useValue: environment },
    AllorsService,
    AuthenticationService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
