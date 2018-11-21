import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NavigationService } from './navigation.service';
export { NavigationService } from './navigation.service';

@NgModule({
  declarations: [
  ],
  exports: [
  ],
  imports: [
    CommonModule,
  ],
  providers: [
    NavigationService
  ]
})
export class NavigationModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: NavigationModule,
      providers: [
        NavigationService
      ]
    };
  }
}
