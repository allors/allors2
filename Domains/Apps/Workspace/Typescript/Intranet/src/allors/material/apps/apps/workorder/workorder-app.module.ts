import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule,  MatIconModule, MatToolbarModule, MatCardModule, MatChipsModule, MatListModule, MatTableModule, MatFormFieldModule, MatSelectModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFilterModule, AllorsMaterialTableModule, AllorsMaterialFactoryFabModule } from '../../..';

import { WorkerOrderMasterComponent } from './master/workorder-master.component';
export { WorkerOrderMasterComponent } from './master/workorder-master.component';

import { WorkerOrderDetailComponent } from './detail/workorder-detail.component';
import { AllorsMaterialModelAutoCompleteModule } from 'src/allors/material/base/components/model/autocomplete/autocomplete.module';
export { WorkerOrderDetailComponent } from './detail/workorder-detail.component';

@NgModule({
  declarations: [
    WorkerOrderMasterComponent,
    WorkerOrderDetailComponent,
  ],
  exports: [
    WorkerOrderMasterComponent,
    WorkerOrderDetailComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatIconModule,
    MatListModule,
    MatTableModule,
    MatToolbarModule,
    AllorsMaterialFactoryFabModule,
    AllorsMaterialFilterModule,
    AllorsMaterialTableModule,
    AllorsMaterialModelAutoCompleteModule,
  ],
})
export class WorkOrdersAppModule { }
