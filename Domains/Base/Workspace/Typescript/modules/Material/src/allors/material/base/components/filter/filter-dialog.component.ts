import { MatDialogRef, MAT_DIALOG_DATA, MatStepper } from '@angular/material';
import { Component, Inject, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { AllorsFilterService } from '../../../../angular/base/filter';
import { FilterField } from 'src/allors/angular/base/filter/FilterField';
import { ParametrizedPredicate } from 'src/allors/framework';

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
      predicate: ['', Validators.required],
      value: ['', Validators.required]
    });
  }

  get selectedPredicate(): ParametrizedPredicate {
    return this.formGroup.get('predicate').value;
  }

  apply() {
    this.filterService.addFilterField(new FilterField({
      predicate: this.formGroup.get('predicate').value,
      value: this.formGroup.get('value').value
    }));

    this.dialogRef.close();
  }

}


