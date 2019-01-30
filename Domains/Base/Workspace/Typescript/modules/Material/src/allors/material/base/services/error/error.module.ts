import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material';
import { AllorsMaterialErrorDialogModule } from '../../components/errordialog';

import { ErrorService } from '../../../../angular';

import { AllorsMaterialDefaultErrorService } from './error.service';
export { AllorsMaterialDefaultErrorService } from './error.service';

import { ErrorConfig } from './error.config';

@NgModule({
  declarations: [
  ],
  exports: [
  ],
  imports: [
    CommonModule,
    MatDialogModule,
    AllorsMaterialErrorDialogModule,

  ],
  providers: [
    AllorsMaterialDefaultErrorService
  ]
})
export class ErrorModule {
  static forRoot(config: ErrorConfig): ModuleWithProviders {
    return {
      ngModule: ErrorModule,
      providers: [
        { provide: ErrorService, useClass: AllorsMaterialDefaultErrorService },
        { provide: ErrorConfig, useValue: config },
      ]
    };
  }
}
