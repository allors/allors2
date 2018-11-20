import { NgModule, ModuleWithProviders } from '@angular/core';
import { MatSnackBarModule } from '@angular/material';
import { AllorsMaterialDialogModule } from '../../components/dialog';

import { NavigateService } from './navigate.service';
export { NavigateService } from './navigate.service';

@NgModule({
  declarations: [
  ],
  exports: [
  ],
  imports: [
  ],
  providers: [
    NavigateService
  ]
})
export class NavigateModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: NavigateModule,
      providers: [ NavigateService ]
    };
  }
}
