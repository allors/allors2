import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { MatAutocompleteModule, MatButtonModule, MatCardModule, MatIconModule, MatListModule, MatMenuModule, MatToolbarModule } from "@angular/material";
import { CovalentCommonModule, CovalentLayoutModule, CovalentLoadingModule } from "@covalent/core";

import { ChipsModule } from "@allors/base-covalent";
import { StaticModule } from "@allors/base-material";

import { PeopleComponent } from "./people.component";
export { PeopleComponent } from "./people.component";

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
    CovalentLayoutModule,
    CovalentCommonModule,
    CovalentLoadingModule,
    ChipsModule,
    StaticModule,
  ],
})
export class PeopleModule {}
