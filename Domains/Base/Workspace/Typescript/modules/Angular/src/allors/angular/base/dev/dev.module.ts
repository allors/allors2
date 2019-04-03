import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DevService } from './dev.service';
export { DevService } from './dev.service';

import { AllorsDevComponent } from './dev.component';
export { AllorsDevComponent } from './dev.component';

@NgModule({
  declarations: [
    AllorsDevComponent
  ],
  exports: [
    AllorsDevComponent
  ],
  imports: [
    CommonModule
  ],
  providers: [
    DevService
  ]
})
export class AllorsDevModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: AllorsDevModule,
      providers: [ DevService ]
    };
  }
}
