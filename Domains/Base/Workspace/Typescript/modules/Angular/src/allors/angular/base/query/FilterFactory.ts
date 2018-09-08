import { Observable } from 'rxjs/Observable';

import 'rxjs/add/observable/empty';

import { And, Exists, ISessionObject, Like, MetaObjectType, Not, ObjectType, Or, PullRequest, Pull, RoleType, Sort, Filter, Result } from '../../../framework';

import { Loaded } from '../framework/responses/Loaded';
import { Scope } from '../framework/Scope';

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
        return Observable.empty();
      }

      const terms: string[] = search.trim().split(' ');

      const and: And = new And();

      if (this.options.existRoletypes) {
        this.options.existRoletypes.forEach((roleType: RoleType) => {
          and.operands.push(new Exists({ propertyType: roleType }));
        });
      }

      if (this.options.notExistRoletypes) {
        this.options.notExistRoletypes.forEach((roleType: RoleType) => {
          const not = new Not();
          and.operands.push(not);
          not.operand = new Exists({ propertyType: roleType });
        });
      }

      terms.forEach((term: string) => {
        const or: Or = new Or();
        and.operands.push(or);
        this.options.roleTypes.forEach((roleType: RoleType) => {
          or.operands.push(new Like({ roleType, value: term + '%' }));
        });
      });

      if (this.options.post) {
        this.options.post(and);
      }

      const pulls = [
        new Pull({
          extent: new Filter({
            objectType: this.options.objectType,
            predicate: and,
            sort: this.options.roleTypes.map((roleType: RoleType) => new Sort({ roleType })),
            }),
          results: [
            new Result({
              name: 'results',
            })
          ]
        }),
      ];

      return scope
        .load('Pull', new PullRequest({ pulls }))
        .map((loaded: Loaded) => loaded.collections.results);
    };
  }
}
