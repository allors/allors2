import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AllorsRefreshService } from './refresh.service';
export { AllorsRefreshService } from './refresh.service';

@NgModule({
  declarations: [
  ],
  exports: [
  ],
  imports: [
  ],
  providers: [
    AllorsRefreshService
  ]
})
export class AllorsRefreshModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: AllorsRefreshModule,
      providers: [ AllorsRefreshService ]
    };
  }
}
