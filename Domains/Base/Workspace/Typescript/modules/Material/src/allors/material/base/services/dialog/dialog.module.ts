import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material';

import { AllorsMaterialDialogService } from './dialog.service';
export { AllorsMaterialDialogService } from './dialog.service';

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
    AllorsMaterialDialogService
  ]
})
export class DialogModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: DialogModule,
      providers: [ AllorsMaterialDialogService ]
    };
  }
}
