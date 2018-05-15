import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MatAutocompleteModule, MatButtonModule, MatCardModule, MatIconModule, MatListModule, MatToolbarModule } from '@angular/material';

import { AutoCompleteModule, InputModule, SelectModule, StaticModule, ChipsModule } from '../../../../../material';

import { PersonComponent } from './person.component';
export { PersonComponent } from './person.component';

@NgModule({
  declarations: [
    PersonComponent,
  ],
  exports: [
    PersonComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatListModule,
    MatToolbarModule,
    AutoCompleteModule,
    ChipsModule,
    InputModule,
    SelectModule,
    StaticModule,
  ],
})
export class PersonModule {}
