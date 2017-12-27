import { Component , Input, Optional } from "@angular/core";
import { NgForm } from "@angular/forms";

import { Field } from "../../../../angular";

@Component({
  selector: "a-mat-static",
  templateUrl: "./static.component.html",
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

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }
}
