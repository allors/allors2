import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule, MatIconModule,  MatMenuModule, MatToolbarModule } from '@angular/material';

import { FactoryFabComponent } from './factoryfab.component';
export { FactoryFabComponent } from './factoryfab.component';

@NgModule({
  declarations: [
    FactoryFabComponent,
  ],
  exports: [
    FactoryFabComponent,

  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatToolbarModule
  ],
})
export class AllorsMaterialFactoryFabModule { }
