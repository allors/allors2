import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatTabsModule, MatExpansionModule, MatDatepickerModule } from '@angular/material';

import { AllorsMaterialFileModule } from '../../../../../base/components/role/file';
import { AllorsMaterialHeaderModule } from '../../../../../base/components/header';
import { AllorsMaterialInputModule } from '../../../../../base/components/role/input';
import { AllorsMaterialSelectModule } from '../../../../../base/components/role/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../../base/components/role/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../../base/components/role/static';
import { AllorsMaterialTextAreaModule } from '../../../../../base/components/role/textarea';
import { AllorsMaterialFooterModule } from '../../../../../base/components/footer';
import { AllorsMaterialLocalisedTextModule } from '../../../../../base/components/role/localisedtext';
import { AllorsMaterialFilesModule } from '../../../../../base/components/role/files';
import { AllorsMaterialDatepickerModule } from '../../../../../base/components/role/datepicker';

import { NonUnifiedGoodOverviewDetailComponent } from './nonunifiedgood-overview-detail.component';
export { NonUnifiedGoodOverviewDetailComponent as GoodOverviewDetailComponent } from './nonunifiedgood-overview-detail.component';

@NgModule({
  declarations: [
    NonUnifiedGoodOverviewDetailComponent,
  ],
  exports: [
    NonUnifiedGoodOverviewDetailComponent,
  ],
  imports: [
    AllorsMaterialDatepickerModule,
    AllorsMaterialFileModule,
    AllorsMaterialFilesModule,
    AllorsMaterialHeaderModule,
    AllorsMaterialFooterModule,
    AllorsMaterialInputModule,
    AllorsMaterialLocalisedTextModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTextAreaModule,
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatDatepickerModule,
    MatDividerModule,
    MatExpansionModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatRadioModule,
    MatSelectModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
    MatOptionModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class NonUnifiedGoodOverviewDetailModule { }
