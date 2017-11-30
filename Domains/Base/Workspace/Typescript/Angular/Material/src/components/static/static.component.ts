import { Component , Input } from "@angular/core";

import { Field } from "@allors/base-angular";

@Component({
  selector: "a-mat-static",
  template: `
<mat-form-field style="width: 100%;" fxLayout="column" fxLayoutAlign="top stretch">
  <input matInput type="type" [ngModel]="static" [name]="name" [placeholder]="label" readonly>
</mat-form-field>
`,
})
export class StaticComponent extends Field {
  @Input()
  public display: string;

  get static(): string {
    if (this.ExistObject) {
      if (this.roleType.objectType.isUnit) {
        return this.model;
      } else {
        if (this.roleType.isOne) {
          if (this.model) {
            return this.model[this.display];
          }
        } else {
          const roles: any[] = this.model;
          if (roles && roles.length > 0) {
            return roles
              .map((v: any) => v[this.display])
              .reduce((acc: string, cur: string) => acc + ", " + cur);
          }
        }
      }
    }
  }
}
