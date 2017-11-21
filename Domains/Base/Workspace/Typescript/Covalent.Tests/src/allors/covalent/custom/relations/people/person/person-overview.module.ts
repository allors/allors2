import { CommonModule} from "@angular/common";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { MatButtonModule, MatCardModule, MatIconModule, MatToolbarModule } from "@angular/material";
import { CovalentLayoutModule } from "@covalent/core";

import { StaticModule } from "@baseMaterial/index";

import { PersonOverviewComponent } from "./person-overview.component";
export { PersonOverviewComponent } from "./person-overview.component";

@NgModule({
  declarations: [
    PersonOverviewComponent,
  ],
  exports: [
    PersonOverviewComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    CovalentLayoutModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatToolbarModule,
    StaticModule,
  ],
})
export class PersonOverviewModule {}
