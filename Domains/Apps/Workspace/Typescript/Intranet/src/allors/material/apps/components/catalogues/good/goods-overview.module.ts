import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AllorsMaterialFileModule } from '../../../../base/components/file';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule } from '@angular/material';
import { RouterModule } from '@angular/router';
import { AllorsMaterialStaticModule } from '../../../../base/components/static';
import { AllorsMaterialSelectModule, AllorsMaterialInputModule, AllorsMaterialTextAreaModule } from '../../../..';

import { GoodsOverviewComponent } from './goods-overview.component';
export { GoodsOverviewComponent } from './goods-overview.component';

@NgModule({
  declarations: [
    GoodsOverviewComponent,
  ],
  exports: [
    GoodsOverviewComponent,
  ],
  imports: [
    AllorsMaterialFileModule,
    AllorsMaterialInputModule,
    AllorsMaterialSelectModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTextAreaModule,
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatDividerModule,
    MatFormFieldModule,  
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatRadioModule,
    MatSelectModule,
    MatToolbarModule,
    MatTooltipModule,
    MatOptionModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class GoodsOverviewModule {}
