import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatStepper } from '@angular/material/stepper';
import { Component, Inject, ViewChild, OnInit, AfterViewInit, ElementRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { timer } from 'rxjs';

import { FilterBuilder } from '../../../../angular/core/filter';
import { FilterField } from '../../../../../allors/angular/core/filter/FilterField';
import { FilterFieldDefinition } from '../../../../../allors/angular/core/filter/FilterFieldDefinition';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { assert } from '../../../../../allors/framework';
import { AllorsFocusService } from '../../../../../allors/angular';

@Component({
  templateUrl: 'filter-dialog.component.html',
})
export class AllorsMaterialFilterDialogComponent implements OnInit {
  @ViewChild('stepper', { static: true }) stepper: MatStepper;

  @ViewChild('focus1') focus1: ElementRef;
  @ViewChild('focus2') focus2: ElementRef;
  @ViewChild('focus3') focus3: ElementRef;
  @ViewChild('focus4') focus4: ElementRef;

  builder: FilterBuilder;

  formGroup: FormGroup;

  filterFieldDefinition: FilterFieldDefinition | null;

  constructor(
    public dialogRef: MatDialogRef<AllorsMaterialFilterDialogComponent>,
    private formBuilder: FormBuilder,
    private focusService: AllorsFocusService,
    @Inject(MAT_DIALOG_DATA) data: any,
  ) {
    this.builder = data.filterBuilder;
  }

  get isBetween() {
    return this.filterFieldDefinition?.isBetween ?? false;
  }

  get placeholder() {
    return this.isBetween ? 'From' : 'Value';
  }

  get useSearch(): boolean {
    return !!this.filterFieldDefinition?.options?.search;
  }

  get useToggle(): boolean {
    return !this.filterFieldDefinition?.options?.search && (this.filterFieldDefinition?.predicate.objectType.isBoolean ?? false);
  }

  get useDatepicker(): boolean {
    return !this.filterFieldDefinition?.options?.search && (this.filterFieldDefinition?.predicate.objectType.isDateTime ?? false);
  }

  get useInput(): boolean {
    return (
      !this.filterFieldDefinition?.options?.search &&
      (!this.filterFieldDefinition?.predicate.objectType.isBoolean ?? false) &&
      (!this.filterFieldDefinition?.predicate.objectType.isDateTime ?? false)
    );
  }

  ngOnInit() {
    this.formGroup = this.formBuilder.group({
      definition: ['', Validators.required],
      value: ['', Validators.required],
      value2: ['', Validators.required],
    });
  }

  focus() {
    setTimeout(() => {
      this.focus1?.nativeElement.focus();
      this.focus2?.nativeElement.focus();
      this.focus3?.nativeElement.focus();
      this.focus4?.nativeElement.focus();
    }, 0);
  }

  stepperSelectionChange(event: StepperSelectionEvent) {
    if (event.selectedIndex === 0) {
      this.filterFieldDefinition = null;
    }
  }

  selected(filterFieldDefinition: FilterFieldDefinition) {
    this.filterFieldDefinition = filterFieldDefinition;

    let initialValue = filterFieldDefinition.options?.initialValue;
    if (initialValue != null) {
      if (filterFieldDefinition.predicate.objectType.isBoolean) {
        initialValue = true;
      }
    }

    const valueControl = this.formGroup.get('value');
    assert(valueControl);
    valueControl.setValue(initialValue);

    // give angular time to process the [completed] directive
    timer(1).subscribe((v) => this.stepper.next());
  }

  apply() {
    assert(this.filterFieldDefinition);

    const objectType = this.filterFieldDefinition.predicate.objectType;
    const options = this.filterFieldDefinition.options;

    let value = this.formGroup.get('value')?.value;
    let value2 = this.formGroup.get('value2')?.value;

    const inValid = value == null || (objectType.isComposite && value.objectType == null);

    if (!inValid) {
      if (objectType.isDateTime) {
        value = value ? value.toISOString() : null;
        value2 = value2 ? value2.toISOString() : null;
      }

      if (!value2) {
        this.builder.addFilterField(
          new FilterField({
            definition: this.filterFieldDefinition,
            value: value.id ? value.id : value,
            display: options?.display(value) ?? value,
          }),
        );
      } else {
        this.builder.addFilterField(
          new FilterField({
            definition: this.filterFieldDefinition,
            value,
            value2,
            display: options?.display ? `${options.display(value)} <-> ${options.display(value2)}` : `${value} <-> ${value2}`,
          }),
        );
      }
    }

    this.dialogRef.close();
  }
}
