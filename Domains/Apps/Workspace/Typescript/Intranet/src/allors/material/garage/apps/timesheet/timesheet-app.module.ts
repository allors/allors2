import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule,  MatIconModule, MatToolbarModule, MatCardModule, MatChipsModule, MatListModule, MatTableModule, MatFormFieldModule, MatSelectModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFilterModule, AllorsMaterialTableModule, AllorsMaterialFactoryFabModule, AllorsMaterialModelAutoCompleteModule } from '../../..';

import { CalendarModule } from 'angular-calendar';

import { TimesheetAppComponent  } from './timesheet-app.component';
export { TimesheetAppComponent  } from './timesheet-app.component';

@NgModule({
  declarations: [
    TimesheetAppComponent,
  ],
  exports: [
    TimesheetAppComponent,
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
    CalendarModule,
    AllorsMaterialFactoryFabModule,
    AllorsMaterialFilterModule,
    AllorsMaterialTableModule,
    AllorsMaterialModelAutoCompleteModule,
  ],
})
export class TimesheetAppModule { }
