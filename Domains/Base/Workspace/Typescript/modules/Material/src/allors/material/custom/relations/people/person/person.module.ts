import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MatAutocompleteModule, MatButtonModule, MatCardModule, MatIconModule, MatListModule, MatToolbarModule } from '@angular/material';

import { AutoCompleteModule, ChipsModule, FileModule, FilesModule, InputModule, SelectModule, StaticModule } from '../../../../../material';

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
    FileModule,
    FilesModule,
    InputModule,
    SelectModule,
    StaticModule,
  ],
})
export class PersonModule {}
