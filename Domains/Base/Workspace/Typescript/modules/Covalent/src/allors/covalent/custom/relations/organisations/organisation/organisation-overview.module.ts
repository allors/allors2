import { CommonModule} from "@angular/common";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { MatButtonModule, MatCardModule, MatIconModule, MatListModule, MatToolbarModule } from "@angular/material";
import { CovalentLayoutModule } from "@covalent/core";

import { StaticModule } from "../../../../../material";

import { OrganisationOverviewComponent } from "./organisation-overview.component";
export { OrganisationOverviewComponent } from "./organisation-overview.component";

@NgModule({
  declarations: [
    OrganisationOverviewComponent,
  ],
  exports: [
    OrganisationOverviewComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    CovalentLayoutModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatListModule,
    MatToolbarModule,
    StaticModule,
  ],
})
export class OrganisationOverviewModule {}
