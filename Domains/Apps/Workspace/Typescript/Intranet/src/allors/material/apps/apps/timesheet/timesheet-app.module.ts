import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule,  MatIconModule, MatToolbarModule, MatCardModule, MatChipsModule, MatListModule, MatTableModule, MatFormFieldModule, MatSelectModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFilterModule, AllorsMaterialTableModule, AllorsMaterialFactoryFabModule, AllorsMaterialModelAutoCompleteModule } from '../../..';

import { TimesheetMasterComponent } from './master/timesheet-master.component';
export { TimesheetMasterComponent } from './master/timesheet-master.component';

import { TimesheetDetailComponent } from './detail/timesheet-detail.component';
export { TimesheetDetailComponent } from './detail/timesheet-detail.component';

@NgModule({
  declarations: [
    TimesheetMasterComponent,
    TimesheetDetailComponent,
  ],
  exports: [
    TimesheetMasterComponent,
    TimesheetDetailComponent,
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
export class TimesheetAppModule { }
