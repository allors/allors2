import { MatDialogRef, MAT_DIALOG_DATA, MatStepper, MatChipListChange } from '@angular/material';
import { Component, Inject, ViewChild, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { timer } from 'rxjs';

import { AllorsFilterService } from '../../../../angular/base/filter';
import { FilterField } from '../../../../../allors/angular/base/filter/FilterField';
import { FilterFieldDefinition } from '../../../../../allors/angular/base/filter/filterFieldDefinition';
import { StepperSelectionEvent } from '@angular/cdk/stepper';

@Component({
  templateUrl: 'filter-dialog.component.html',
})
export class AllorsMaterialFilterDialogComponent implements OnInit {

  @ViewChild('stepper') stepper: MatStepper;

  filterService: AllorsFilterService;

  formGroup: FormGroup;

  filterFieldDefinition: FilterFieldDefinition;

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

  stepperSelectionChange(event: StepperSelectionEvent) {
    if (event.selectedIndex === 0) {
      this.filterFieldDefinition = undefined;
    }
  }

  selected(filterFieldDefinition: FilterFieldDefinition) {
    this.filterFieldDefinition = filterFieldDefinition;
    // give angular time to process the [completed] directive
    timer(1).subscribe((v) => this.stepper.next());
  }

  apply() {

    const options = this.filterFieldDefinition && this.filterFieldDefinition.options;
    const value = this.formGroup.get('value').value;

    this.filterService.addFilterField(new FilterField({
      definition: this.filterFieldDefinition,
      value: value.id ? value.id : value,
      display: options.display ? options.display(value) : value,
    }));

    this.dialogRef.close();
  }

}


