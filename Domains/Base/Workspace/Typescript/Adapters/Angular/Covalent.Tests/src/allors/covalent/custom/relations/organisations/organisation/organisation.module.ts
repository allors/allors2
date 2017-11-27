import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { CovalentLayoutModule } from "@covalent/core";

import { MatAutocompleteModule, MatButtonModule, MatCardModule, MatIconModule, MatListModule, MatToolbarModule } from "@angular/material";

import { ChipsModule } from "@allors/base-covalent";
import { AutoCompleteModule, InputModule, StaticModule } from "@allors/base-material";

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
    MatListModule,
    MatToolbarModule,
    CovalentLayoutModule,
    AutoCompleteModule,
    ChipsModule,
    InputModule,
    StaticModule,
  ],
})
export class OrganisationModule {}
