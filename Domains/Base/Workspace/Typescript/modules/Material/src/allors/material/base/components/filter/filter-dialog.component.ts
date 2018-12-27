import { MatDialogRef, MAT_DIALOG_DATA, MatStepper } from '@angular/material';
import { Component, Inject, ViewChild, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { AllorsFilterService } from '../../../../angular/base/filter';
import { FilterField } from '../../../../../allors/angular/base/filter/FilterField';
import { FilterFieldDefinition } from '../../../../../allors/angular/base/filter/filterFieldDefinition';

@Component({
  templateUrl: 'filter-dialog.component.html',
})
export class AllorsMaterialFilterDialogComponent implements OnInit {

  @ViewChild('stepper') stepper: MatStepper;

  filterService: AllorsFilterService;

  formGroup: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<AllorsMaterialFilterDialogComponent>,
    private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) data: any) {

    this.filterService = data.filterService;
  }

  ngOnInit() {
    this.formGroup = this.formBuilder.group({
      definition: ['', Validators.required],
      value: ['', Validators.required]
    });
  }

  get definition(): FilterFieldDefinition {
    return this.formGroup.get('definition').value;
  }

  apply() {
    this.filterService.addFilterField(new FilterField({
      definition: this.definition,
      value: this.formGroup.get('value').value
    }));

    this.dialogRef.close();
  }

}


