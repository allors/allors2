import { ChangeDetectorRef, Component , Input } from "@angular/core";
import { Field } from "@baseAngular";
import { ISessionObject } from "@baseDomain";
import { RoleType } from "@baseMeta";

@Component({
  selector: "a-mat-static",
  template: `
<mat-input-container fxLayout="column" fxLayoutAlign="top stretch">
  <input matInput type="type" [ngModel]="static" [name]="name" [placeholder]="label" readonly>
</mat-input-container>
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
          return this.model[this.display];
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
