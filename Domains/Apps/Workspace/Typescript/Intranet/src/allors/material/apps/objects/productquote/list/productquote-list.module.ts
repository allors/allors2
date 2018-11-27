import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule,  MatIconModule, MatToolbarModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFilterModule, AllorsMaterialTableModule} from '../../../..';

import { ProductQuotesOverviewComponent } from './productquote-list.component';
export { ProductQuotesOverviewComponent } from './productquote-list.component';

@NgModule({
  declarations: [
    ProductQuotesOverviewComponent,
  ],
  exports: [
    ProductQuotesOverviewComponent,
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
export class ProductQuotesOverviewModule {}
