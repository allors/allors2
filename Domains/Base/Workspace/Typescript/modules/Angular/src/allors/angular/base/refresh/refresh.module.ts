import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RefreshService } from './refresh.service';
export { RefreshService } from './refresh.service';

@NgModule({
  declarations: [
  ],
  exports: [
  ],
  imports: [
  ],
  providers: [
    RefreshService
  ]
})
export class AllorsRefreshModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: AllorsRefreshModule,
      providers: [ RefreshService ]
    };
  }
}
