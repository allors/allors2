// tslint:disable: directive-selector
// tslint:disable: directive-class-suffix
import { AfterViewInit, Component, Input, OnDestroy, QueryList, ViewChildren, Directive } from '@angular/core';
import { NgForm, NgModel } from '@angular/forms';

import { RoleType, assert, humanize } from '@allors/meta/system';
import { ISessionObject } from '@allors/domain/system';

import { Field } from './Field';

@Directive()
export abstract class RoleField extends Field implements AfterViewInit, OnDestroy {
  @Input()
  public object: ISessionObject;

  @Input()
  public roleType: RoleType;

  @Input()
  assignedRoleType: RoleType;

  @Input()
  derivedInitialRole: ISessionObject;

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
    if (this.ExistObject) {
      if (this.assignedRoleType) {
        if (this.object.hasChangedRole(this.assignedRoleType)) {
          return this.object.get(this.assignedRoleType);
        }

        if (this.object.isNew && this.derivedInitialRole) {
          return this.derivedInitialRole;
        }
      }

      return this.object.get(this.roleType);
    }

    return undefined;
  }

  set model(value: any) {
    if (this.ExistObject) {
      if (this.emptyStringIsNull && value === '') {
        value = null;
      }

      if (value && value.toISOString) {
        if (typeof value.toISOString === 'function') {
          value = (value as any).toISOString();
        }
      }

      if (this.roleType.objectType.isDecimal) {
        value = (value as string)?.replace(',', '.');
      }

      if (this.assignedRoleType) {
        if (this.object.isNew && this.derivedInitialRole && this.derivedInitialRole === value) {
          this.object.set(this.assignedRoleType, null);
        } else {
          this.object.set(this.assignedRoleType, value);
        }
      } else {
        this.object.set(this.roleType, value);
      }
    }
  }

  get canRead(): boolean | undefined {
    return this.object?.canRead(this.roleType);
  }

  get canWrite(): boolean | undefined {
    return this.object?.canWrite(this.assignedRoleType ?? this.roleType);
  }

  get textType(): string {
    if (this.roleType.objectType.name === 'Integer' || this.roleType.objectType.name === 'Float') {
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
    if (this.assignedRequired) {
      return this.assignedRequired;
    }

    if (this.object) {
      return this.roleType.isRequired(this.object.objectType);
    }

    return false;
  }

  get disabled(): boolean {
    return !this.canWrite || !!this.assignedDisabled;
  }

  get canRestore(): boolean {
    return this.ExistObject && this.assignedRoleType && this.object.hasChangedRole(this.assignedRoleType);
  }

  restore(): void {
    this.ExistObject && this.object?.restoreRole(this.assignedRoleType);
  }

  public add(value: ISessionObject) {
    if (this.ExistObject) {
      this.object.add(this.roleType, value);
    }
  }

  public remove(value: ISessionObject) {
    if (this.ExistObject) {
      this.object.remove(this.roleType, value);
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
    return this.object?.id;
  }

  get dataAllorsRoleType(): string {
    return this.roleType.id;
  }
}
