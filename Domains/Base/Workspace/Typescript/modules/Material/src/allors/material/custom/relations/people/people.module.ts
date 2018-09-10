import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { MatAutocompleteModule, MatButtonModule, MatCardModule, MatIconModule, MatListModule,
         MatMenuModule, MatToolbarModule } from '@angular/material';

import { AllorsMaterialChipsModule, AllorsMaterialStaticModule, AllorsMaterialSelectModule, AllorsMaterialSideNavToggleModule } from '../../../../material';

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
    MatCardModule,
    MatIconModule,
    MatListModule,
    MatMenuModule,
    MatToolbarModule,

    AllorsMaterialChipsModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialSelectModule,
  ],
})
export class PeopleModule {}
