import { NgModule, ModuleWithProviders } from '@angular/core';
import { DateConfig } from './date.config';
import { DateService } from './date.service';

@NgModule({
  providers: [
    DateService,
  ]
})
export class DateModule {
  static forRoot(config: Partial<DateConfig>): ModuleWithProviders<DateModule> {
    return {
      ngModule: DateModule,
      providers: [
        DateService,
        { provide: DateConfig, useValue: config },
      ]
    };
  }
}
