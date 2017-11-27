import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { CovalentLayoutModule } from "@covalent/core";

import { MatAutocompleteModule, MatButtonModule, MatCardModule, MatIconModule, MatListModule, MatToolbarModule } from "@angular/material";

import { ChipsModule } from "@allors/base-covalent";
import { AutoCompleteModule, InputModule, SelectModule, StaticModule } from "@allors/base-material";

import { PersonComponent } from "./person.component";
export { PersonComponent } from "./person.component";

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
    CovalentLayoutModule,
    AutoCompleteModule,
    ChipsModule,
    InputModule,
    SelectModule,
    StaticModule,
  ],
})
export class PersonModule {}
