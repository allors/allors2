import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatGridListModule, MatCheckboxModule, MatChipsModule, MatButtonToggleModule, MatDialogModule, MatExpansionModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFileModule } from '../../../../base/components/role/file';
import { AllorsMaterialHeaderModule } from '../../../../base/components/header';
import { AllorsMaterialInputModule } from '../../../../base/components/role/input';
import { AllorsMaterialLauncherModule } from '../../../..';
import { AllorsMaterialSelectModule } from '../../../../base/components/role/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../base/components/role/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../base/components/role/static';
import { AllorsMaterialTextAreaModule } from '../../../../base/components/role/textarea';

import { PriceComponentOverviewPanelModule } from '../../pricecomponent/overview/panel/pricecomponent-overview-panel.module';
import { ProductIdentificationPanelModule } from '../../productidentification/overview/panel/productIdentification-panel.module';

import { NonUnifiedGoodOverviewSummaryModule } from './summary/nonunifiedgood-overview-summary.module';
import { NonUnifiedGoodOverviewDetailModule } from './detail/nonunifiedgood-overview-detail.module';

import { NonUnifiedGoodOverviewComponent } from './nonunifiedgood-overview.component';
export { NonUnifiedGoodOverviewComponent } from './nonunifiedgood-overview.component';

@NgModule({
  declarations: [
    NonUnifiedGoodOverviewComponent,
  ],
  exports: [
    NonUnifiedGoodOverviewComponent,
  ],
  imports: [
    NonUnifiedGoodOverviewSummaryModule,
    NonUnifiedGoodOverviewDetailModule,

    ProductIdentificationPanelModule,
    PriceComponentOverviewPanelModule,

    AllorsMaterialFileModule,
    AllorsMaterialHeaderModule,
    AllorsMaterialInputModule,
    AllorsMaterialLauncherModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTextAreaModule,
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDialogModule,
    MatDividerModule,
    MatExpansionModule,
    MatFormFieldModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatRadioModule,
    MatSelectModule,
    MatToolbarModule,
    MatTooltipModule,
    MatOptionModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class NonUnifiedGoodOverviewModule { }
