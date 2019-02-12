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

import { IGoodIdentificationPanelModule } from '../../igoodidentification/overview/panel/igoodIdentification-panel.module';
import { NonSerialisedInventoryItemOverviewPanelModule } from '../../nonserialisedinventoryitem/overview/panel/nonserialisedinventoryitem-overview-panel.module';
import { PriceComponentOverviewPanelModule } from '../../pricecomponent/overview/panel/pricecomponent-overview-panel.module';
import { SerialisedInventoryItemOverviewPanelModule } from '../../serialisedinventoryitem/overview/panel/serialisedinventoryitem-overview-panel.module';
import { SerialisedItemOverviewPanelModule } from '../../serialiseditem/overview/panel/serialiseditem-overview-panel.module';
import { SupplierOfferingOverviewPanelModule } from '../../supplieroffering/overview/panel/supplieroffering-overview-panel.module';

import { NonUnifiedPartOverviewSummaryModule } from './summary/nonunifiedpart-overview-summary.module';
import { NonUnifiedPartOverviewDetailModule } from './detail/nonunifiedpart-overview-detail.module';

import { NonUnifiedPartOverviewComponent } from './nonunifiedpart-overview.component';
export { NonUnifiedPartOverviewComponent } from './nonunifiedpart-overview.component';

@NgModule({
  declarations: [
    NonUnifiedPartOverviewComponent,
  ],
  exports: [
    NonUnifiedPartOverviewComponent,
  ],
  imports: [
    NonUnifiedPartOverviewSummaryModule,
    NonUnifiedPartOverviewDetailModule,

    IGoodIdentificationPanelModule,
    NonSerialisedInventoryItemOverviewPanelModule,
    PriceComponentOverviewPanelModule,
    SerialisedInventoryItemOverviewPanelModule,
    SerialisedItemOverviewPanelModule,
    SupplierOfferingOverviewPanelModule,

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
export class NonUnifiedPartOverviewModule { }
