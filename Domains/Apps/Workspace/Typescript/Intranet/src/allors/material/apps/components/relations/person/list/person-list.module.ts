import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule,  MatIconModule, MatToolbarModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFilterModule, AllorsMaterialTableModule} from '../../../../../../material';

import { PersonListComponent } from './person-list.component';
export { PersonListComponent } from './person-list.component';

@NgModule({
  declarations: [
    PersonListComponent,
  ],
  exports: [
    PersonListComponent,
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
export class PersonListModule { }
