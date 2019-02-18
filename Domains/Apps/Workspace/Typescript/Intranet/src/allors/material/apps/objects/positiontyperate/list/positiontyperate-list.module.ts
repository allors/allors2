import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule,  MatIconModule, MatToolbarModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFilterModule, AllorsMaterialTableModule, AllorsMaterialFactoryFabModule} from '../../../..';

import { PositionTypeRatesOverviewComponent } from './positiontyperate-list.component';
export { PositionTypeRatesOverviewComponent } from './positiontyperate-list.component';

@NgModule({
  declarations: [
    PositionTypeRatesOverviewComponent,
  ],
  exports: [
    PositionTypeRatesOverviewComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    AllorsMaterialFactoryFabModule,
    AllorsMaterialFilterModule,
    AllorsMaterialTableModule,
  ],
})
export class PositionTypeRatesOverviewModule { }
