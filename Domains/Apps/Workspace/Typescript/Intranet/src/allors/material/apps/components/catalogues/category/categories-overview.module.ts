import { NgModule } from '@angular/core';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CdkTableModule } from '@angular/cdk/table';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatTableModule, MatSortModule, MatPaginatorModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFileModule } from '../../../../base/components/file';
import { AllorsMaterialInputModule } from '../../../../base/components/input';
import { AllorsMaterialSearchModule } from '../../../../base/components/search';
import { AllorsMaterialSearchInputModule } from '../../../../base/components/search/input';
import { AllorsMaterialSearchSelectModule } from '../../../../base/components/search/select';
import { AllorsMaterialSelectModule } from '../../../../base/components/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../base/components/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../base/components/static';
import { AllorsMaterialTextAreaModule } from '../../../../base/components/textarea';

import { InternalOrganisationSelectModule } from '../../common/internalorganisation/internalorganisation-select.module';

import { CategoriesOverviewComponent } from './categories-overview.component';
export { CategoriesOverviewComponent } from './categories-overview.component';

@NgModule({
  declarations: [
    CategoriesOverviewComponent,
  ],
  exports: [
    CategoriesOverviewComponent,
  ],
  imports: [
    AllorsMaterialFileModule,
    AllorsMaterialInputModule,
    AllorsMaterialSearchModule,
    AllorsMaterialSearchInputModule,
    AllorsMaterialSearchSelectModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTextAreaModule,
    CommonModule,
    CdkTableModule,
    FormsModule,
    InternalOrganisationSelectModule,
    MatButtonModule,
    MatCardModule,
    MatDividerModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatPaginatorModule,
    MatRadioModule,
    MatSelectModule,
    MatSortModule,
    MatTableModule,
    MatToolbarModule,
    MatTooltipModule,
    MatOptionModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class CategoriesOverviewModule { }
