import { NgModule, ModuleWithProviders } from '@angular/core';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { AllorsMaterialDialogModule } from '../../../components/dialog';

import { MethodService } from './method.service';
export { MethodService } from './method.service';

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
    MethodService
  ]
})
export class MethodModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: MethodModule,
      providers: [ MethodService ]
    };
  }
}
