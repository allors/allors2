import { Observable } from "rxjs/Observable";

import "rxjs/add/observable/empty";

import { And, Exists, ISessionObject, Like, MetaObjectType, Not, ObjectType, Or, PullRequest, Query, RoleType, Sort } from "../../../framework";

import { Loaded } from "../framework/responses/Loaded";
import { Scope } from "../framework/Scope";

export interface FilterOptions {
  objectType: ObjectType | MetaObjectType;
  roleTypes: RoleType[];
  existRoletypes?: RoleType[];
  notExistRoletypes?: RoleType[];
  post?: (and: And) => void;
}

export class FilterFactory {
  constructor(private options: FilterOptions) { }

  public create(scope: Scope): ((search: string) => Observable<ISessionObject[]>) {
    return (search: string) => {
      if (!search.trim) {
        return Observable.empty<ISessionObject[]>();
      }

      const terms: string[] = search.trim().split(" ");

      const and: And = new And();

      if (this.options.existRoletypes) {
        this.options.existRoletypes.forEach((roleType: RoleType) => {
          and.predicates.push(new Exists({ roleType }));
        });
      }

      if (this.options.notExistRoletypes) {
        this.options.notExistRoletypes.forEach((roleType: RoleType) => {
          const not = new Not();
          and.predicates.push(not);
          not.predicate = new Exists({ roleType });
        });
      }

      terms.forEach((term: string) => {
        const or: Or = new Or();
        and.predicates.push(or);
        this.options.roleTypes.forEach((roleType: RoleType) => {
          or.predicates.push(new Like({ roleType, value: term + "%" }));
        });
      });

      if (this.options.post) {
        this.options.post(and);
      }

      const queries = [
        new Query({
          name: "results",
          objectType: this.options.objectType,
          predicate: and,
          sort: this.options.roleTypes.map((roleType: RoleType) => new Sort({ roleType })),
        }),
      ];

      return scope
        .load("Pull", new PullRequest({ queries }))
        .map((loaded: Loaded) => loaded.collections.results);
    };
  }
}
