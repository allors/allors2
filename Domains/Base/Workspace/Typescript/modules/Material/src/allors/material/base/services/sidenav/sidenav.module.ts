import { NgModule, ModuleWithProviders } from '@angular/core';

import { AllorsMaterialSideNavService } from './sidenav.service';
export { AllorsMaterialSideNavService } from './sidenav.service';

@NgModule({
  declarations: [
  ],
  exports: [
  ],
  imports: [
  ],
  providers: [
    AllorsMaterialSideNavService
  ]
})
export class SideNavModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: SideNavModule,
      providers: [ AllorsMaterialSideNavService ]
    };
  }
}
