import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';

import { CoreModule } from './core.module';
import { AuthModule } from './auth/auth.module';
import { FetchModule } from './fetch/fetch.module';
import { HomeModule } from './home/home.module';
import { QueryModule } from './query/query.module';
import { FormModule } from './form/form.module';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';

@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
  ],
  imports: [
    CoreModule,
    AppRoutingModule,

    AuthModule,
    FetchModule,
    FormModule,
    HomeModule,
    QueryModule
  ],
})
export class AppModule { }
