import { HttpClientModule } from '@angular/common/http';
import { NgModule, Optional, SkipSelf } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { MAT_AUTOCOMPLETE_DEFAULT_OPTIONS } from '@angular/material/autocomplete';

import { environment } from '../environments/environment';

import { AllorsDateAdapter } from '../allors/material/core/dateadapter';

@NgModule({
  imports: [
    BrowserModule,
    environment.production ? BrowserAnimationsModule : NoopAnimationsModule,
    RouterModule,
    HttpClientModule,
 ],
  providers: [
    {
      provide: MAT_AUTOCOMPLETE_DEFAULT_OPTIONS,
      useValue: { autoActiveFirstOption: true },
    },
    { provide: DateAdapter, useClass: AllorsDateAdapter },
    {
      provide: MAT_DATE_FORMATS,
      useValue: {
        parse: {
          dateInput: 'dd/MM/yyyy',
        },
        display: {
          dateInput: 'dd/MM/yyyy',
          monthYearLabel: 'LLL y',
          dateA11yLabel: 'MMMM d, y',
          monthYearA11yLabel: 'MMMM y',
        },
      },
    },
  ],
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() core: CoreModule) {
    if (core) {
      throw new Error('Only use CoreModule from AppModule');
    }
  }
}
