import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material';

import { LoggingService } from '../../../../angular';

import { DefaultLoggingService } from './logging.service';
export { DefaultLoggingService } from './logging.service';

import { LoggingConfig } from './logging.config';

@NgModule({
  declarations: [
  ],
  exports: [
  ],
  imports: [
    CommonModule,
    MatDialogModule
  ],
  providers: [
    DefaultLoggingService
  ]
})
export class LoggingModule {
  static forRoot(config: LoggingConfig): ModuleWithProviders {
    return {
      ngModule: LoggingModule,
      providers: [
        { provide: LoggingService, useClass: DefaultLoggingService },
        { provide: LoggingConfig, useValue: config },
      ]
    };
  }
}
