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

import { AllorsAngularModule } from '../angular/module';

import { AllorsMaterialAssociationAutoCompleteComponent } from './components/association/autocomplete';
export { AllorsMaterialAssociationAutoCompleteComponent };

import { AllorsMaterialDialogComponent } from './components/dialog';
export { AllorsMaterialDialogComponent };

import { AllorsMaterialErrorDialogComponent } from './services/save';
export { AllorsMaterialErrorDialogComponent };

import { AllorsMaterialFilterFieldDialogComponent } from '../core/material/components/filter/field/dialog.component';
import { AllorsMaterialFilterFieldSearchComponent } from '../core/material/components/filter/field/search.component';
import { AllorsMaterialFilterComponent } from './components/filter';
export { AllorsMaterialFilterComponent } from './components/filter';

import { AllorsMaterialFooterComponent } from './components/footer';
export { AllorsMaterialFooterComponent };
import { AllorsMaterialFooterSaveCancelComponent } from './components/footer/savecancel';
export { AllorsMaterialFooterSaveCancelComponent };

import { AllorsMaterialHeaderComponent } from './components/header';
export { AllorsMaterialHeaderComponent };

import { AllorsMaterialLauncherComponent } from './components/launcher';
export { AllorsMaterialLauncherComponent };

import { AllorsMaterialMediaComponent } from './components/media';
export { AllorsMaterialMediaComponent };
import { AllorMediaPreviewComponent } from '../core/material/components/media/preview/media-preview.component';

import { AllorsMaterialModelAutocompleteComponent } from './components/model/autocomplete';
export { AllorsMaterialModelAutocompleteComponent };

import { AllorsMaterialAutocompleteComponent } from './components/role/autocomplete';
export { AllorsMaterialAutocompleteComponent };

import { AllorsMaterialCheckboxComponent } from './components/role/checkbox';
export { AllorsMaterialCheckboxComponent };

import { AllorsMaterialChipsComponent } from './components/role/chips';
export { AllorsMaterialChipsComponent };

import { AllorsMaterialDatepickerComponent } from './components/role/datepicker';
export { AllorsMaterialDatepickerComponent };

import { AllorsMaterialDatetimepickerComponent } from './components/role/datetimepicker';
export { AllorsMaterialDatetimepickerComponent };

import { AllorsMaterialFileComponent } from './components/role/file';
export { AllorsMaterialFileComponent };

import { AllorsMaterialFilesComponent } from './components/role/files';
export { AllorsMaterialFilesComponent };

import { AllorsMaterialInputComponent } from './components/role/input';
export { AllorsMaterialInputComponent };

import { AllorsMaterialLocalisedMarkdownComponent } from './components/role/localisedmarkdown';
export { AllorsMaterialLocalisedMarkdownComponent };

import { AllorsMaterialLocalisedTextComponent } from './components/role/localisedtext';
export { AllorsMaterialLocalisedTextComponent };

import { AllorsMaterialMarkdownComponent } from './components/role/markdown';
export { AllorsMaterialMarkdownComponent };

import { AllorsMaterialMonthpickerComponent } from './components/role/monthpicker';
export { AllorsMaterialMonthpickerComponent };

import { AllorsMaterialRadioGroupComponent } from './components/role/radiogroup';
export { AllorsMaterialRadioGroupComponent };

import { AllorsMaterialSelectComponent } from './components/role/select';
export { AllorsMaterialSelectComponent };

import { AllorsMaterialSliderComponent } from './components/role/slider';
export { AllorsMaterialSliderComponent };

import { AllorsMaterialSlideToggleComponent } from './components/role/slidetoggle';
export { AllorsMaterialSlideToggleComponent };

import { AllorsMaterialStaticComponent } from './components/role/static';
export { AllorsMaterialStaticComponent };

import { AllorsMaterialTextareaComponent } from './components/role/textarea';
export { AllorsMaterialTextareaComponent };

import { AllorsMaterialScannerComponent } from './components/scanner';
export { AllorsMaterialScannerComponent };

import { AllorsMaterialSideMenuComponent } from './components/sidemenu';
export { AllorsMaterialSideMenuComponent };

import { AllorsMaterialSideNavToggleComponent } from './components/sidenavtoggle';
export { AllorsMaterialSideNavToggleComponent };

import { AllorsMaterialTableComponent } from './components/table';
export { AllorsMaterialTableComponent };

import { AllorsMaterialDialogService } from './services/dialog';
export { AllorsMaterialDialogService };

import { FactoryFabComponent } from './components/factoryfab';
export { FactoryFabComponent };

// Custom
import { LoginComponent } from '../custom/material/auth';
export { LoginComponent }
import { MainComponent } from '../custom/material/main';
export { MainComponent }
import { DashboardComponent } from '../custom/material/dashboard';
export { DashboardComponent }

import { OrganisationOverviewComponent } from '../custom/material/relations/organisations/organisation';
export { OrganisationOverviewComponent };
import { OrganisationComponent } from '../custom/material/relations/organisations/organisation';
export { OrganisationComponent };
import { OrganisationsComponent } from '../custom/material/relations/organisations';
export { OrganisationsComponent };

import { PersonOverviewComponent } from '../custom/material/relations/people/person';
export { PersonOverviewComponent };
import { PersonComponent } from '../custom/material/relations/people/person';
export { PersonComponent };
import { PeopleComponent } from '../custom/material/relations/people';
export { PeopleComponent };
import { FormComponent } from '../custom/material/tests/form';
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
    LoginComponent,
    MainComponent,
    DashboardComponent,
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
    LoginComponent,
    MainComponent,
    DashboardComponent,
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
