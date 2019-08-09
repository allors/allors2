import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AllorsFocusService } from './focus.service';

import { AllorsFocusDirective } from './focus.directive';
export { AllorsFocusDirective } from './focus.directive';

@NgModule({
  declarations: [
    AllorsFocusDirective,
  ],
  exports: [
    AllorsFocusDirective,
  ],
  imports: [
    CommonModule,
  ],
  providers: [
    AllorsFocusService
  ]
})
export class AllorsFocusModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: AllorsFocusModule,
      providers: [ AllorsFocusService ]
    };
  }
}
