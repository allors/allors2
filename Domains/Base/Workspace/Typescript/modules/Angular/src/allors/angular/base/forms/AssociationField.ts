import { AfterViewInit, Input, OnDestroy, QueryList, ViewChildren } from '@angular/core';
import { NgForm, NgModel } from '@angular/forms';
import { ISessionObject, AssociationType } from '../../../framework';
import { humanize } from '../humanize';
import { Field } from './Field';

export abstract class AssociationField extends Field implements AfterViewInit, OnDestroy {

  @Input()
  public object: ISessionObject;

  @Input()
  public associationType: AssociationType;

  @ViewChildren(NgModel) private controls: QueryList<NgModel>;

  private id = 0;

  constructor(private parentForm: NgForm) {
    super();
    // TODO: wrap around
    this.id = ++Field.counter;
  }

  get existObject(): boolean {
    return !!this.object;
  }

  get model(): any {
    if (this.existObject) {
      const roleType = this.associationType.relationType.roleType;
      return undefined;
    }
  }

  set model(value: any) {
    if (this.existObject) {
    }
  }

  get name(): string {
    return this.associationType.name + '_' + this.id;
  }

  get label(): string {
    return humanize(this.associationType.name);
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

  get dataAllorsAssociationType(): string {
    return this.associationType ? this.associationType.id : null;
  }

}
