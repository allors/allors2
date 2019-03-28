import { Injectable } from "@angular/core";
import { Router } from "@angular/router";

import { ISessionObject, ObjectType } from "../../../framework";
import { ObjectData } from "src/allors/material";

@Injectable()
export class NavigationService {
  constructor(private router: Router) {}

  list(objectType: ObjectType) {
    const url = objectType.list;
    this.router.navigate([url]);
  }

  overview(obj: ISessionObject | ObjectData) {
    const url = obj.objectType.overview.replace(`:id`, obj.id);
    this.router.navigate([url]);
  }

  back(): any {
    throw new Error("Remove this method");
  }
}
