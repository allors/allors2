import { AfterViewInit, Component, Input, OnDestroy, QueryList, ViewChildren } from '@angular/core';
import { NgForm, NgModel } from '@angular/forms';
import { ISessionObject, RoleType } from '../../../framework';
import { humanize } from '../humanize';
import { Field } from './Field';

export abstract class RoleField extends Field implements AfterViewInit, OnDestroy {

  @Input()
  public object: ISessionObject;

  @Input()
  public roleType: RoleType;

  // tslint:disable-next-line:no-input-rename
  @Input('name')
  public assignedName: string;

  // tslint:disable-next-line:no-input-rename
  @Input('disabled')
  public assignedDisabled: boolean;

  // tslint:disable-next-line:no-input-rename
  @Input('required')
  public assignedRequired: boolean;

  // tslint:disable-next-line:no-input-rename
  @Input('label')
  public assignedLabel: string;

  @Input()
  public readonly: boolean;

  @Input()
  public hint: string;

  @Input()
  public focus: boolean;

  @Input()
  public emptyStringIsNull = true;

  @ViewChildren(NgModel) private controls: QueryList<NgModel>;

  private id = 0;

  constructor(private parentForm: NgForm) {
    super();
    // TODO: wrap around
    this.id = ++Field.counter;
  }

  get ExistObject(): boolean {
    return !!this.object;
  }

  get model(): any {
    return this.ExistObject ? this.object.get(this.roleType.name) : undefined;
  }

  set model(value: any) {
    if (this.ExistObject) {

      if (this.emptyStringIsNull && value === '') {
        value = null;
      }

      this.object.set(this.roleType.name, value);
    }
  }

  get canRead(): boolean {
    if (this.ExistObject) {
      return this.object.canRead(this.roleType.name);
    }
  }

  get canWrite(): boolean {
    if (this.ExistObject) {
      return this.object.canWrite(this.roleType.name);
    }
  }

  get textType(): string {
    if (this.roleType.objectType.name === 'Integer' ||
      this.roleType.objectType.name === 'Decimal' ||
      this.roleType.objectType.name === 'Float') {
      return 'number';
    }

    return 'text';
  }

  get name(): string {
    return this.assignedName ? this.assignedName : this.roleType.name + '_' + this.id;
  }

  get label(): string {
    return this.assignedLabel ? this.assignedLabel : humanize(this.roleType.name);
  }

  get required(): boolean {
    return this.assignedRequired ? this.assignedRequired : this.roleType.isRequired;
  }

  get disabled(): boolean {
    return !this.canWrite || !!this.assignedDisabled;
  }

  public add(value: ISessionObject) {
    if (this.ExistObject) {
      this.object.add(this.roleType.name, value);
    }
  }

  public remove(value: ISessionObject) {
    if (this.ExistObject) {
      this.object.remove(this.roleType.name, value);
    }
  }

  public ngAfterViewInit(): void {
    if (!!this.parentForm) {
      this.controls.forEach((control: NgModel) => {
        this.parentForm.addControl(control);
      });
    }
  }

  public ngOnDestroy(): void {
    if (!!this.parentForm) {
      this.controls.forEach((control: NgModel) => {
        this.parentForm.removeControl(control);
      });
    }
  }

  get dataAllorsId(): string {
    return this.object ? this.object.id : null;
  }

  get dataAllorsRoleType(): string {
    return this.roleType ? this.roleType.id : null;
  }

}
