import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { CovalentLayoutModule } from "@covalent/core";

import { MatAutocompleteModule, MatButtonModule, MatCardModule, MatIconModule, MatToolbarModule } from "@angular/material";

import { ChipsModule } from "@baseCovalent/index";
import { AutoCompleteModule, InputModule, StaticModule } from "@baseMaterial/index";

import { OrganisationComponent } from "./organisation.component";
export { OrganisationComponent } from "./organisation.component";

@NgModule({
  declarations: [
    OrganisationComponent,
  ],
  exports: [
    OrganisationComponent,
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
    StaticModule,
  ],
})
export class OrganisationModule {}
