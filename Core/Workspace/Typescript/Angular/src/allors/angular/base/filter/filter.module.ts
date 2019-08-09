import { NgModule, ModuleWithProviders } from '@angular/core';

import { AllorsFilterService } from './filter.service';
export { AllorsFilterService } from './filter.service';


@NgModule({
  declarations: [
  ],
  exports: [
  ],
  entryComponents: [
  ],
  imports: [
  ],
})
export class AllorsFilterModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: AllorsFilterModule,
      providers: [ AllorsFilterService ]
    };
  }
}
