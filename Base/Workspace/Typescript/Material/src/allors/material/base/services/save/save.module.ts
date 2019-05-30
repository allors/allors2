import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material/dialog';

import { AllorsMaterialErrorDialogModule } from './error/errordialog.module';

import { SaveService } from './save.service';
export { SaveService } from './save.service';


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
    SaveService
  ]
})
export class SaveModule {
}
