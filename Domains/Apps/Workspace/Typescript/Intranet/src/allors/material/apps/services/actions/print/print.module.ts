import { NgModule, ModuleWithProviders } from '@angular/core';
import { MatSnackBarModule } from '@angular/material';

import { PrintService } from './print.service';
import { AllorsMaterialDialogModule } from '../../../../base/components/dialog';
export { PrintService } from './print.service';

@NgModule({
  declarations: [
  ],
  exports: [
  ],
  imports: [
    MatSnackBarModule,
    AllorsMaterialDialogModule,
  ],
  providers: [
    PrintService
  ]
})
export class PrintModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: PrintModule,
      providers: [ PrintService ]
    };
  }
}
