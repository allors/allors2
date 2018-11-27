import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule, MatGridListModule, MatCheckboxModule, MatChipsModule, MatButtonToggleModule, MatDialogModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { AllorsMaterialFileModule } from '../../../../base/components/file';
import { AllorsMaterialHeaderModule } from '../../../../base/components/header';
import { AllorsMaterialInputModule } from '../../../../base/components/input';
import { AllorsMaterialSelectModule } from '../../../../base/components/select';
import { AllorsMaterialSideNavToggleModule } from '../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../base/components/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../base/components/static';
import { AllorsMaterialTextAreaModule } from '../../../../base/components/textarea';

import { NonSerialisedInventoryEmbedModule } from '../../nonserialisedinventoryitem/embed/nonserialisedinventoryitem-embed.module';

import { SerialisedItemEmbedModule } from '../../serialiseditem/embed/serialiseditem-embed.module';
import { IGoodIdentificationEmbedModule } from '../../igoodidentification/embed/igoodIdentification-embed.module';
import { PriceComponentEmbedModule } from '../../pricecomponent/embed/pricecomponent-embed.module';
import { SerialisedInventoryEmbedModule } from '../../serialisedinventoryitem/embed/serialisedinventoryitem-embed.module';

import { PartOverviewComponent } from './part-overview.component';
export { PartOverviewComponent } from './part-overview.component';

@NgModule({
  declarations: [
    PartOverviewComponent,
  ],
  exports: [
    PartOverviewComponent,
  ],
  imports: [
    SerialisedItemEmbedModule,
    IGoodIdentificationEmbedModule,
    PriceComponentEmbedModule,
    SerialisedInventoryEmbedModule,
    NonSerialisedInventoryEmbedModule,

    AllorsMaterialFileModule,
    AllorsMaterialHeaderModule,
    AllorsMaterialInputModule,
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
export class PartOverviewModule { }
