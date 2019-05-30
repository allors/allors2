import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';

import { AllorsMaterialChipsModule, AllorsMaterialDialogModule, AllorsMaterialFilterModule, AllorsMaterialStaticModule, AllorsMaterialSelectModule, AllorsMaterialSideNavToggleModule } from '../../../../material';

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
    AllorsMaterialDialogModule,
    AllorsMaterialFilterModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialSelectModule,
  ],
})
export class PeopleModule { }
