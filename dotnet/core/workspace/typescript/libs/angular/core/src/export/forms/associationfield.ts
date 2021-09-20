// tslint:disable: directive-selector
// tslint:disable: directive-class-suffix
import { AfterViewInit, Input, OnDestroy, QueryList, ViewChildren, Directive } from '@angular/core';
import { NgForm, NgModel } from '@angular/forms';

import { AssociationType, RoleType, assert, humanize } from '@allors/meta/system';
import { ISessionObject } from '@allors/domain/system';

import { Field } from './Field';

@Directive()
export abstract class AssociationField extends Field implements AfterViewInit, OnDestroy {
  @Input()
  object: ISessionObject;

  @Input()
  get associationType(): AssociationType {
    return this._associationType;
  }

  set associationType(associationType: AssociationType) {
    assert(!associationType || associationType.isOne, 'AssociationType should have one multiplicity');
    this._associationType = associationType;
  }

  @Input()
  hint: string;
  
  // tslint:disable-next-line:no-input-rename
  @Input('label')
  public assignedLabel: string;
  
  @ViewChildren(NgModel) private controls: QueryList<NgModel>;

  get roleType(): RoleType {
    return this.associationType?.relationType.roleType;
  }

  private id = 0;

  private _associationType: AssociationType;

  constructor(private parentForm: NgForm) {
    super();
    // TODO: wrap around
    this.id = ++Field.counter;
  }

  get existObject(): boolean {
    return !!this.object;
  }

  get model(): ISessionObject | undefined {
    const model = this.existObject && this.associationType ? this.object.getAssociation(this.associationType) : undefined;

    return model;
  }

  set model(association: ISessionObject | undefined) {
    if (this.existObject) {
      const prevModel = this.model;

      if (prevModel && prevModel !== association) {
        if (this.roleType.isOne) {
          prevModel.set(this.roleType, null);
        } else {
          prevModel.remove(this.roleType, this.object);
        }
      }

      if (association) {
        if (this.roleType.isOne) {
          association.set(this.roleType, this.object);
        } else {
          association.add(this.roleType, this.object);
        }
      }
    }
  }

  get name(): string {
    return this.associationType.name + '_' + this.id;
  }

  get label(): string | undefined {
    return this.assignedLabel ? this.assignedLabel : humanize(this.associationType.name);
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
    return this.object.id;
  }

  get dataAllorsAssociationType(): string {
    return this.associationType.id;
  }
}
