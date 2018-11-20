import { NgModule, ModuleWithProviders } from '@angular/core';
import { MatSnackBarModule } from '@angular/material';
import { AllorsMaterialDialogModule } from '../../components/dialog';

import { DeleteService } from './delete.service';

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
    DeleteService
  ]
})
export class DeleteModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: DeleteModule,
      providers: [ DeleteService ]
    };
  }
}
