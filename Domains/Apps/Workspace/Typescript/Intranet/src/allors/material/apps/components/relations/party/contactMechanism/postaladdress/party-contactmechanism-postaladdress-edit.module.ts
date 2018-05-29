import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule, MatCardModule, MatDividerModule, MatFormFieldModule, MatIconModule, MatListModule, MatMenuModule, MatRadioModule, MatToolbarModule, MatTooltipModule, MatOptionModule, MatSelectModule, MatInputModule } from '@angular/material';


import { AllorsMaterialFileModule } from '../../../../../../base/components/file';
import { AllorsMaterialInputModule } from '../../../../../../base/components/input';
import { AllorsMaterialSelectModule } from '../../../../../../base/components/select';
import { AllorsMaterialSlideToggleModule } from '../../../../../../base/components/slidetoggle';
import { AllorsMaterialStaticModule } from '../../../../../../base/components/static';
import { AllorsMaterialTextAreaModule } from '../../../../../../base/components/textarea';

import { PartyContactMechanismPostalAddressEditComponent } from './party-contactmechanism-postaladdress-edit.component';
export { PartyContactMechanismPostalAddressEditComponent } from './party-contactmechanism-postaladdress-edit.component';

@NgModule({
  declarations: [
    PartyContactMechanismPostalAddressEditComponent,
  ],
  exports: [
    PartyContactMechanismPostalAddressEditComponent,
  ],
  imports: [
    
    AllorsMaterialFileModule,
    AllorsMaterialInputModule,
    AllorsMaterialSelectModule,
    AllorsMaterialSlideToggleModule,
    AllorsMaterialStaticModule,
    AllorsMaterialTextAreaModule,
    CommonModule,
    FormsModule,
    MatButtonModule,
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
    MatOptionModule,
    ReactiveFormsModule,
    RouterModule,
  ],
})
export class PartyContactMechanismPostalAddressEditModule {}
