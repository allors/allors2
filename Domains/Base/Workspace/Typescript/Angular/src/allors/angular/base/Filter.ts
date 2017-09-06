import { Observable } from "rxjs/Rx";

import { And, ISessionObject, Like, Or, PullRequest, Query, Sort } from "../../domain";
import { ObjectType, ObjectTyped, RoleType } from "../../meta";
import { Loaded } from "./responses/Loaded";
import { Scope } from "./Scope";

export class Filter {

  constructor(public scope: Scope, public objectType: ObjectType | ObjectTyped, public roleTypes: RoleType[]) { }

  public create(): ((search: string) => Observable<ISessionObject[]>) {
    return (search: string) => {
      const terms: string[] = search.trim().split(" ");

      const and: And = new And();
      terms.forEach((term: string) => {
        const or: Or = new Or();
        and.predicates.push(or);
        this.roleTypes.forEach((roleType: RoleType) => {
          or.predicates.push(new Like({ roleType, value: term + "%" }));
        });
      });

      const query: Query[] = [
        new Query(
          {
            name: "results",
            objectType: this.objectType,
            predicate: and,
            sort: this.roleTypes.map((roleType: RoleType) => new Sort({ roleType })),
          }),
      ];

      return this.scope
        .load("Pull", new PullRequest({ query }))
        .map((loaded: Loaded) => loaded.collections.results);
    };
  }
}
