import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AllorsBarcodeService } from './barcode.service';

import { AllorsBarcodeDirective } from './barcode.directive';
export { AllorsBarcodeDirective } from './barcode.directive';

@NgModule({
  declarations: [
    AllorsBarcodeDirective,
  ],
  exports: [
    AllorsBarcodeDirective,
  ],
  imports: [
    CommonModule,
  ],
  providers: [
    AllorsBarcodeService
  ]
})
export class AllorsBarcodeModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: AllorsBarcodeModule,
      providers: [ AllorsBarcodeService ]
    };
  }
}
