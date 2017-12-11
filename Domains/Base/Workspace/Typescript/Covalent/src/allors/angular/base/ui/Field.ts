import { Component, Input } from "@angular/core";
import { ISessionObject, RoleType } from "../../../framework";

export abstract class Field {
  @Input()
  public object: ISessionObject;

  @Input()
  public roleType: RoleType;

  @Input("label")
  public assignedLabel: string;

  @Input()
  public hint: string;

  @Input("disabled")
  public assignedDisabled: boolean;

  @Input("readonly")
  public assignedReadonly: boolean;

  @Input("required")
  public assignedRequired: boolean;

  get ExistObject(): boolean {
    return !!this.object;
  }

  get model(): any {
    return this.ExistObject ? this.object.get(this.roleType.name) : undefined;
  }

  set model(value: any) {
    if (this.ExistObject) {
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
    if (this.roleType.objectType.name === "Integer" ||
      this.roleType.objectType.name === "Decimal" ||
      this.roleType.objectType.name === "Float") {
      return "number";
    }

    return "text";
  }

  get name(): string {
    return this.roleType.name;
  }

  get label(): string {
    return this.assignedLabel ? this.assignedLabel : this.humanize(this.roleType.name);
  }

  get disabled(): boolean {
    return !this.canWrite ? true : this.assignedDisabled;
  }

  get readonly(): boolean {
    return !this.canWrite ? true : this.assignedReadonly;
  }

  get required(): boolean {
    return this.roleType.isRequired ? true : this.assignedRequired;
  }

  protected humanize(input: string): string {
    return input ? input.replace(/([a-z\d])([A-Z])/g, "$1 $2")
      .replace(/([A-Z]+)([A-Z][a-z\d]+)/g, "$1 $2")
      : undefined;
  }
}
