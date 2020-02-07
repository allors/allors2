import { NgModule, ModuleWithProviders } from '@angular/core';

import { OverviewService } from './overview.service';
export { OverviewService } from './overview.service';

@NgModule({
  declarations: [
  ],
  exports: [
  ],
  imports: [
  ],
  providers: [
    OverviewService
  ]
})
export class NavigateModule {
  static forRoot(): ModuleWithProviders<NavigateModule> {
    return {
      ngModule: NavigateModule,
      providers: [ OverviewService ]
    };
  }
}
