import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';

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
