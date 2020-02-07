import { NgModule, ModuleWithProviders } from '@angular/core';

import { PrintService } from './print.service';
import { PrintConfig } from './print.config';
export { PrintService } from './print.service';

@NgModule({
  declarations: [
  ],
  exports: [
  ],
  imports: [
  ],
  providers: [
    PrintService
  ]
})
export class PrintModule {
  static forRoot(config: PrintConfig): ModuleWithProviders<PrintModule> {
    return {
      ngModule: PrintModule,
      providers: [
        PrintService,
        { provide: PrintConfig, useValue: config },
      ]
    };
  }
}
