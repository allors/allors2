import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule,  MatIconModule, MatToolbarModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFilterModule, AllorsMaterialTableModule} from '../../../..';

import { SalesOrdersOverviewComponent } from './salesorder-list.component';
export { SalesOrdersOverviewComponent } from './salesorder-list.component';

@NgModule({
  declarations: [
    SalesOrdersOverviewComponent,
  ],
  exports: [
    SalesOrdersOverviewComponent,
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
export class SalesOrdersOverviewModule { }
