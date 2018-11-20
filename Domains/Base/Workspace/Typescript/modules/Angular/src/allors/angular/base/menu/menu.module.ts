import { NgModule, ModuleWithProviders } from '@angular/core';

import { MenuService } from './menu.service';
export { MenuService } from './menu.service';

@NgModule({
  declarations: [
  ],
  exports: [
  ],
  imports: [
  ],
  providers: [
    MenuService
  ]
})
export class MenuModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: MenuModule,
      providers: [
        MenuService
      ]
    };
  }
}
