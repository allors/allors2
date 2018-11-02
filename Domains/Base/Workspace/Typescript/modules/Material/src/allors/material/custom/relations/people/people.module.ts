import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import {
  MatAutocompleteModule, MatButtonModule, MatCardModule, MatIconModule, MatListModule,
  MatMenuModule, MatToolbarModule, MatCheckboxModule, MatTableModule, MatSortModule
} from '@angular/material';

import { AllorsMaterialChipsModule, AllorsMaterialFilterModule, AllorsMaterialStaticModule, AllorsMaterialSelectModule, AllorsMaterialSideNavToggleModule } from '../../../../material';

import { PeopleComponent } from './people.component';
export { PeopleComponent } from './people.component';

@NgModule({
  declarations: [
    PeopleComponent,
  ],
  exports: [
    PeopleComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,

    MatAutocompleteModule,
    MatButtonModule,
    MatCheckboxModule,
    MatCardModule,
    MatIconModule,
    MatListModule,
    MatMenuModule,
    MatSortModule,
    MatTableModule,
    MatToolbarModule,

    AllorsMaterialChipsModule,
    AllorsMaterialFilterModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialSelectModule,
  ],
})
export class PeopleModule { }
