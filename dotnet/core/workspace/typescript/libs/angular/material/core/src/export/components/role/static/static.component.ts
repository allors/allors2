import { Component , Input, Optional } from '@angular/core';
import { NgForm } from '@angular/forms';

import { RoleField } from '@allors/angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-static',
  templateUrl: './static.component.html',
})
export class AllorsMaterialStaticComponent extends RoleField {
  @Input()
  public display: string;

  get static(): string | null {
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
              .reduce((acc: string, cur: string) => acc + ', ' + cur);
          }
        }
      }
    }

    return null;
  }

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }
}
