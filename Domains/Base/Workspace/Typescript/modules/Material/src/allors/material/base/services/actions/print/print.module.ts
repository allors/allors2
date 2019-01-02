import { NgModule, ModuleWithProviders } from '@angular/core';
import { MatSnackBarModule } from '@angular/material';
import { AllorsMaterialDialogModule } from '../../../components/dialog';

import { PrintService } from './print.service';
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
