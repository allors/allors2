import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule,  MatIconModule, MatToolbarModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFilterModule, AllorsMaterialTableModule} from '../../../..';

import { ProductCharacteristicsOverviewComponent } from './productcharacteristic-list.component';
export { ProductCharacteristicsOverviewComponent } from './productcharacteristic-list.component';

@NgModule({
  declarations: [
    ProductCharacteristicsOverviewComponent,
  ],
  exports: [
    ProductCharacteristicsOverviewComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    AllorsMaterialFilterModule,
    AllorsMaterialTableModule,
  ],
})
export class ProductCharacteristicsOverviewModule { }
