import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FlexLayoutModule } from "@angular/flex-layout";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpModule } from "@angular/http";
import { RouterModule } from "@angular/router";

const ANGULAR_MODULES: any[] = [
  FlexLayoutModule, HttpModule, FormsModule, ReactiveFormsModule, RouterModule,
];

import {
  MatAutocompleteModule, MatButtonModule, MatCardModule, MatCheckboxModule, MatDatepickerModule,
  MatIconModule, MatInputModule, MatListModule, MatMenuModule,
  MatNativeDateModule, MatRadioModule, MatSelectModule,
  MatSidenavModule, MatSliderModule, MatSlideToggleModule,
  MatSnackBarModule, MatTabsModule, MatToolbarModule, MatTooltipModule,
} from "@angular/material";

const MATERIAL_MODULES: any[] = [
  MatAutocompleteModule, MatButtonModule, MatCardModule, MatCheckboxModule, MatDatepickerModule,
  MatIconModule, MatInputModule, MatListModule, MatMenuModule,
  MatNativeDateModule, MatRadioModule, MatSelectModule,
  MatSidenavModule, MatSliderModule, MatSlideToggleModule,
  MatSnackBarModule, MatTabsModule, MatToolbarModule, MatTooltipModule,
];

import {
  CovalentChipsModule, CovalentCommonModule, CovalentDataTableModule,
  CovalentDialogsModule, CovalentFileModule, CovalentLayoutModule,
  CovalentLoadingModule, CovalentMediaModule, CovalentMenuModule,
  CovalentNotificationsModule, CovalentPagingModule, CovalentSearchModule,
  CovalentStepsModule,
} from "@covalent/core";

const COVALENT_MODULES: any[] = [
  CovalentChipsModule, CovalentCommonModule, CovalentDataTableModule,
  CovalentDialogsModule, CovalentFileModule, CovalentLayoutModule,
  CovalentLoadingModule, CovalentMediaModule, CovalentMenuModule,
  CovalentNotificationsModule, CovalentPagingModule, CovalentSearchModule,
  CovalentStepsModule,
];

import { AutoCompleteModule, CheckboxModule, DatepickerModule, InputModule, LocalisedTextModule,
  RadioGroupModule, SelectModule, SliderModule, SlideToggleModule, StaticModule, TextAreaModule } from "../../material";

const BASE_MATERIAL_MODULES: any[] = [
  AutoCompleteModule, CheckboxModule, DatepickerModule, InputModule, LocalisedTextModule,
  RadioGroupModule, SelectModule, SliderModule, SlideToggleModule, StaticModule, TextAreaModule,
];

import { ChipsModule, MediaImageModule, MediaImagesModule } from "../../covalent";

const BASE_COVALENT_MODULES: any[] = [
  ChipsModule, MediaImageModule, MediaImagesModule,
];

@NgModule({
  exports: [
    ANGULAR_MODULES,
    MATERIAL_MODULES,
    COVALENT_MODULES,

    BASE_MATERIAL_MODULES,
    BASE_COVALENT_MODULES,
  ],
  imports: [
    ANGULAR_MODULES,
    MATERIAL_MODULES,
    COVALENT_MODULES,

    BASE_MATERIAL_MODULES,
    BASE_COVALENT_MODULES,
  ],
})
export class SharedModule { }
