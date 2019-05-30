import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatOptionModule } from '@angular/material/core';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';

import { AllorsMaterialFactoryFabModule } from '../../../../../base/components/factoryfab/factoryfab.module';
import { AllorsMaterialFileModule } from '../../../../../base/components/role/file';
import { AllorsMaterialInputModule } from '../../../../../base/components/role/input';
import { AllorsMaterialSelectModule } from '../../../../../base/components/role/select';
import { AllorsMaterialTableModule } from '../../../../../base/components/table';
import { AllorsMaterialSideNavToggleModule } from '../../../../../base/components/sidenavtoggle';
import { AllorsMaterialSlideToggleModule } from '../../../../../base/components/role/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../../base/components/role/static';
import { AllorsMaterialTextAreaModule } from '../../../../../base/components/role/textarea';

import { WorkEffortOverviewPanelComponent } from './workeffort-overview-panel.component';
export { WorkEffortOverviewPanelComponent } from './workeffort-overview-panel.component';

@NgModule({
  declarations: [
    WorkEffortOverviewPanelComponent,
  ],
  exports: [
    WorkEffortOverviewPanelComponent,
  ],
  imports: [
    AllorsMaterialFactoryFabModule,
    AllorsMaterialFileModule,
    AllorsMaterialInputModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSideNavToggleModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTableModule,
    AllorsMaterialTextAreaModule,
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatDividerModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatRadioModule,
    MatSelectModule,
    MatToolbarModule,
    MatTooltipModule,
    MatButtonToggleModule,
    MatOptionModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class WorkEffortOverviewPanelModule { }
