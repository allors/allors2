import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

const ANGULAR_MODULES: any[] = [
  CommonModule, FlexLayoutModule, HttpModule, FormsModule, ReactiveFormsModule, RouterModule,
];

import {
  MatAutocompleteModule, MatButtonModule, MatButtonToggleModule, MatCardModule, MatCheckboxModule, MatDatepickerModule, MatExpansionModule,
  MatIconModule, MatInputModule, MatListModule, MatMenuModule,
  MatNativeDateModule, MatRadioModule, MatSelectModule,
  MatSidenavModule, MatSliderModule, MatSlideToggleModule,
  MatSnackBarModule, MatTabsModule, MatToolbarModule, MatTooltipModule,
} from '@angular/material';

const MATERIAL_MODULES: any[] = [
  MatAutocompleteModule, MatButtonModule, MatButtonToggleModule, MatCardModule, MatCheckboxModule, MatDatepickerModule, MatExpansionModule,
  MatIconModule, MatInputModule, MatListModule, MatMenuModule,
  MatNativeDateModule, MatRadioModule, MatSelectModule,
  MatSidenavModule, MatSliderModule, MatSlideToggleModule,
  MatSnackBarModule, MatTabsModule, MatToolbarModule, MatTooltipModule,
];

import {
  AutoCompleteModule, CheckboxModule, ChipsModule, DatepickerModule, FileModule, FilesModule, InputModule, LocalisedTextModule,
  RadioGroupModule, SelectModule, SideNavToggleModule, SliderModule, SlideToggleModule, StaticModule, TextAreaModule
} from '../../../material';

const BASE_MATERIAL_MODULES: any[] = [
  AutoCompleteModule, CheckboxModule, ChipsModule, DatepickerModule, FileModule, FilesModule, InputModule, LocalisedTextModule,
  RadioGroupModule, SelectModule, SideNavToggleModule, SliderModule, SlideToggleModule, StaticModule, TextAreaModule,
];

@NgModule({
  exports: [
    ANGULAR_MODULES,
    MATERIAL_MODULES,
    BASE_MATERIAL_MODULES,
  ],
  imports: [
    ANGULAR_MODULES,
    MATERIAL_MODULES,
    BASE_MATERIAL_MODULES,
  ],
})
export class SharedModule { }
