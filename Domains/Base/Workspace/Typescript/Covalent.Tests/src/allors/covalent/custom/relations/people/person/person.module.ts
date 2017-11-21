import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { CovalentLayoutModule } from "@covalent/core";

import { MatAutocompleteModule, MatButtonModule, MatCardModule, MatIconModule, MatToolbarModule } from "@angular/material";

import { ChipsModule } from "@baseCovalent/index";
import { AutoCompleteModule, InputModule, SelectModule, StaticModule } from "@baseMaterial/index";

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
