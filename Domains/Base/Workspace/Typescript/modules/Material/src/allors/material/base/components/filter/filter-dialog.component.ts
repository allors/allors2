import { MatDialogRef, MAT_DIALOG_DATA, MatStepper } from '@angular/material';
import { Component, Inject, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { AllorsFilterService, FilterFieldDefinition } from '../../../../angular/base/filter';

@Component({
  templateUrl: 'filter-dialog.component.html',
})
export class AllorsMaterialFilterDialogComponent {

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

  get selectedDefinition(): FilterFieldDefinition {
    return this.formGroup.get('definition').value;
  }

  apply() {
    this.filterService.addFilterField({
      definition: this.formGroup.get('definition').value,
      value: this.formGroup.get('value').value
    });

    this.dialogRef.close();
  }

}


