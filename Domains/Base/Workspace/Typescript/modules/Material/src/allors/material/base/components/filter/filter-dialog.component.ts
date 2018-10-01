import { MatDialogRef, MAT_DIALOG_DATA, MatStepper } from '@angular/material';
import { Component, Inject, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { AllorsMaterialFilterService } from './filter.service';
import { FilterDefinition } from './FilterDefinition';

@Component({
  templateUrl: 'filter-dialog.component.html',
})
export class AllorsMaterialFilterDialogComponent {

  @ViewChild('stepper') stepper: MatStepper;

  filterService: AllorsMaterialFilterService;

  formGroup: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<AllorsMaterialFilterDialogComponent>,
    private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) data: any) {

    this.filterService = data.filterService;
  }

  get selectedFilterDefinition(): FilterDefinition {
    return this.formGroup.get('definition').value;
  }

  apply() {
  }

  ngOnInit() {
    this.formGroup = this.formBuilder.group({
      definition: ['', Validators.required],
      value: ['', Validators.required]
    });
  }
}


