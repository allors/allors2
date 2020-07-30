import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSliderModule } from '@angular/material/slider';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';

import { AllorsAngularModule } from '../angular';

import { AllorsMaterialAssociationAutoCompleteComponent } from './core/components/association/autocomplete/autocomplete.component';
export { AllorsMaterialAssociationAutoCompleteComponent };

import { AllorsMaterialDialogComponent } from './core/components/dialog/dialog.component';
export { AllorsMaterialDialogComponent };

import { AllorsMaterialErrorDialogComponent } from './core/services/save/error/errordialog.component';
export { AllorsMaterialErrorDialogComponent };

import { AllorsMaterialFilterFieldDialogComponent } from './core/components/filter/field/dialog.component';
import { AllorsMaterialFilterFieldSearchComponent } from './core/components/filter/field/search.component';
import { AllorsMaterialFilterComponent } from './core/components/filter/filter.component';
export { AllorsMaterialFilterComponent } from './core/components/filter/filter.component';

import { AllorsMaterialFooterComponent } from './core/components/footer/footer.component';
export { AllorsMaterialFooterComponent };
import { AllorsMaterialFooterSaveCancelComponent } from './core/components/footer/savecancel/savecancel.component';
export { AllorsMaterialFooterSaveCancelComponent };

import { AllorsMaterialHeaderComponent } from './core/components/header/header.component';
export { AllorsMaterialHeaderComponent };

import { AllorsMaterialLauncherComponent } from './core/components/launcher/launcher.component';
export { AllorsMaterialLauncherComponent };

import { AllorsMaterialMediaComponent } from './core/components/media/media.component';
export { AllorsMaterialMediaComponent };
import { AllorMediaPreviewComponent } from './core/components/media/preview/media-preview.component';

import { AllorsMaterialModelAutocompleteComponent } from './core/components/model/autocomplete/autocomplete.component';
export { AllorsMaterialModelAutocompleteComponent };

import { AllorsMaterialAutocompleteComponent } from './core/components/role/autocomplete/autocomplete.component';
export { AllorsMaterialAutocompleteComponent };

import { AllorsMaterialCheckboxComponent } from './core/components/role/checkbox/checkbox.component';
export { AllorsMaterialCheckboxComponent };

import { AllorsMaterialChipsComponent } from './core/components/role/chips/chips.component';
export { AllorsMaterialChipsComponent };

import { AllorsMaterialDatepickerComponent } from './core/components/role/datepicker/datepicker.component';
export { AllorsMaterialDatepickerComponent };

import { AllorsMaterialDatetimepickerComponent } from './core/components/role/datetimepicker/datetimepicker.component';
export { AllorsMaterialDatetimepickerComponent };

import { AllorsMaterialFileComponent } from './core/components/role/file/file.component';
export { AllorsMaterialFileComponent };

import { AllorsMaterialFilesComponent } from './core/components/role/files/files.component';
export { AllorsMaterialFilesComponent };

import { AllorsMaterialInputComponent } from './core/components/role/input/input.component';
export { AllorsMaterialInputComponent };

import { AllorsMaterialLocalisedMarkdownComponent } from './core/components/role/localisedmarkdown/localisedmarkdown.component';
export { AllorsMaterialLocalisedMarkdownComponent };

import { AllorsMaterialLocalisedTextComponent } from './core/components/role/localisedtext/localisedtext.component';
export { AllorsMaterialLocalisedTextComponent };

import { AllorsMaterialMarkdownComponent } from './core/components/role/markdown/markdown.component';
export { AllorsMaterialMarkdownComponent };

import { AllorsMaterialMonthpickerComponent } from './core/components/role/monthpicker/monthpicker.component';
export { AllorsMaterialMonthpickerComponent };

import { AllorsMaterialRadioGroupComponent } from './core/components/role/radiogroup/radiogroup.component';
export { AllorsMaterialRadioGroupComponent };

import { AllorsMaterialSelectComponent } from './core/components/role/select/select.component';
export { AllorsMaterialSelectComponent };

import { AllorsMaterialSliderComponent } from './core/components/role/slider/slider.component';
export { AllorsMaterialSliderComponent };

import { AllorsMaterialSlideToggleComponent } from './core/components/role/slidetoggle/slidetoggle.component';
export { AllorsMaterialSlideToggleComponent };

import { AllorsMaterialStaticComponent } from './core/components/role/static/static.component';
export { AllorsMaterialStaticComponent };

import { AllorsMaterialTextareaComponent } from './core/components/role/textarea/textarea.component';
export { AllorsMaterialTextareaComponent };

import { AllorsMaterialScannerComponent } from './core/components/scanner/scanner.component';
export { AllorsMaterialScannerComponent };

import { AllorsMaterialSideMenuComponent } from './core/components/sidemenu/sidemenu.component';
export { AllorsMaterialSideMenuComponent };

import { AllorsMaterialSideNavToggleComponent } from './core/components/sidenavtoggle/sidenavtoggle.component';
export { AllorsMaterialSideNavToggleComponent };

import { AllorsMaterialTableComponent } from './core/components/table/table.component';
export { AllorsMaterialTableComponent };

import { AllorsMaterialDialogService } from './core/services/dialog/dialog.service';
export { AllorsMaterialDialogService };

import { FactoryFabComponent } from './core/components/factoryfab/factoryfab.component';
export { FactoryFabComponent };

// Custom
import { OrganisationOverviewComponent } from './custom/relations/organisations/organisation/organisation-overview.component';
export { OrganisationOverviewComponent };

import { OrganisationComponent } from './custom/relations/organisations/organisation/organisation.component';
export { OrganisationComponent };

import { OrganisationsComponent } from './custom/relations/organisations/organisations.component';
export { OrganisationsComponent };

import { PersonOverviewComponent } from './custom/relations/people/person/person-overview.component';
export { PersonOverviewComponent };

import { PersonComponent } from './custom/relations/people/person/person.component';
export { PersonComponent } from './custom/relations/people/person/person.component';

import { PeopleComponent } from './custom/relations/people/people.component';
export { PeopleComponent };

import { FormComponent } from './custom/tests/form/form.component';
export { FormComponent };

@NgModule({
  declarations: [
    // core
    AllorsMaterialAssociationAutoCompleteComponent,
    AllorsMaterialDialogComponent,
    AllorsMaterialErrorDialogComponent,
    AllorsMaterialFilterComponent,
    AllorsMaterialFilterFieldDialogComponent,
    AllorsMaterialFilterFieldSearchComponent,
    AllorsMaterialFooterComponent,
    AllorsMaterialFooterSaveCancelComponent,
    AllorsMaterialHeaderComponent,
    AllorsMaterialLauncherComponent,
    AllorsMaterialMediaComponent,
    AllorMediaPreviewComponent,
    AllorsMaterialModelAutocompleteComponent,
    AllorsMaterialAutocompleteComponent,
    AllorsMaterialCheckboxComponent,
    AllorsMaterialChipsComponent,
    AllorsMaterialDatepickerComponent,
    AllorsMaterialDatetimepickerComponent,
    AllorsMaterialFileComponent,
    AllorsMaterialFilesComponent,
    AllorsMaterialInputComponent,
    AllorsMaterialLocalisedMarkdownComponent,
    AllorsMaterialLocalisedTextComponent,
    AllorsMaterialMarkdownComponent,
    AllorsMaterialMonthpickerComponent,
    AllorsMaterialRadioGroupComponent,
    AllorsMaterialSelectComponent,
    AllorsMaterialSliderComponent,
    AllorsMaterialSlideToggleComponent,
    AllorsMaterialStaticComponent,
    AllorsMaterialTextareaComponent,
    AllorsMaterialScannerComponent,
    AllorsMaterialSideMenuComponent,
    AllorsMaterialSideNavToggleComponent,
    AllorsMaterialTableComponent,
    FactoryFabComponent,
    // custom
    OrganisationOverviewComponent,
    OrganisationComponent,
    OrganisationsComponent,
    PersonOverviewComponent,
    PersonComponent,
    PeopleComponent,
    FormComponent,
  ],
  exports: [
    // core
    AllorsMaterialAssociationAutoCompleteComponent,
    AllorsMaterialDialogComponent,
    AllorsMaterialErrorDialogComponent,
    AllorsMaterialFilterComponent,
    AllorsMaterialFooterComponent,
    AllorsMaterialFooterSaveCancelComponent,
    AllorsMaterialHeaderComponent,
    AllorsMaterialLauncherComponent,
    AllorsMaterialMediaComponent,
    AllorsMaterialModelAutocompleteComponent,
    AllorsMaterialAutocompleteComponent,
    AllorsMaterialCheckboxComponent,
    AllorsMaterialChipsComponent,
    AllorsMaterialDatepickerComponent,
    AllorsMaterialDatetimepickerComponent,
    AllorsMaterialFileComponent,
    AllorsMaterialFilesComponent,
    AllorsMaterialInputComponent,
    AllorsMaterialLocalisedMarkdownComponent,
    AllorsMaterialLocalisedTextComponent,
    AllorsMaterialMarkdownComponent,
    AllorsMaterialMonthpickerComponent,
    AllorsMaterialRadioGroupComponent,
    AllorsMaterialSelectComponent,
    AllorsMaterialSliderComponent,
    AllorsMaterialSlideToggleComponent,
    AllorsMaterialStaticComponent,
    AllorsMaterialTextareaComponent,
    AllorsMaterialScannerComponent,
    AllorsMaterialSideMenuComponent,
    AllorsMaterialSideNavToggleComponent,
    AllorsMaterialTableComponent,
    FactoryFabComponent,
    // custom
    OrganisationOverviewComponent,
    OrganisationComponent,
    OrganisationsComponent,
    PersonOverviewComponent,
    PersonComponent,
    PeopleComponent,
    FormComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    AllorsAngularModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDatepickerModule,
    MatDialogModule,
    MatFormFieldModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatPaginatorModule,
    MatRadioModule,
    MatSelectModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatStepperModule,
    MatTableModule,
    MatToolbarModule,
  ],
})
export class AllorsMaterialModule {}
