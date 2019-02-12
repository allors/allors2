import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule, MatIconModule, MatToolbarModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFilterModule, AllorsMaterialTableModule, AllorsMaterialFactoryFabModule } from '../../../..';

import { UnifiedGoodListComponent } from './unifiedgood-list.component';
export { UnifiedGoodListComponent } from './unifiedgood-list.component';

@NgModule({
  declarations: [
    UnifiedGoodListComponent,
  ],
  exports: [
    UnifiedGoodListComponent,
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
export class UnifiedGoodListModule { }
