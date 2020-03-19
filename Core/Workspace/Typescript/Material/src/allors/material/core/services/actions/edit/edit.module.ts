import { NgModule, ModuleWithProviders } from '@angular/core';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { AllorsMaterialDialogModule } from '../../../components/dialog';

import { EditService } from './edit.service';
export { EditService } from './edit.service';

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
    EditService
  ]
})
export class EditModule {
  static forRoot(): ModuleWithProviders<EditModule> {
    return {
      ngModule: EditModule,
      providers: [ EditService ]
    };
  }
}
