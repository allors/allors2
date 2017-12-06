import { Component, Input } from "@angular/core";
import { ISessionObject, RoleType } from "../../../framework";

export abstract class Field {
  @Input()
  public object: ISessionObject;

  @Input()
  public roleType: RoleType;

  @Input("disabled")
  public assignedDisabled: boolean;

  @Input("required")
  public assignedRequired: boolean;

  @Input("label")
  public assignedLabel: string;

  @Input()
  public readonly: boolean;

  @Input()
  public hint: string;

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

  get required(): boolean {
    return this.assignedRequired ? this.assignedRequired : this.roleType.isRequired;
  }

  get disabled(): boolean {
    return !this.canWrite || this.assignedDisabled;
  }

  protected humanize(input: string): string {
    return input ? input.replace(/([a-z\d])([A-Z])/g, "$1 $2")
      .replace(/([A-Z]+)([A-Z][a-z\d]+)/g, "$1 $2")
      : undefined;
  }
}
