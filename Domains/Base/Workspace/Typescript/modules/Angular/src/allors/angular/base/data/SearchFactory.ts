import { Observable, EMPTY } from 'rxjs';
import { map } from 'rxjs/operators';

import { And, Exists, ISessionObject, Like, MetaObjectType, Not, ObjectType, Or, PullRequest, Pull, RoleType, Sort } from '../../../framework';
import { Loaded, Scope } from '../framework';

export interface SearchOptions {
  objectType: ObjectType | MetaObjectType;
  roleTypes: RoleType[];
  existRoletypes?: RoleType[];
  notExistRoletypes?: RoleType[];
  post?: (and: And) => void;
}

export class SearchFactory {
  constructor(private options: SearchOptions) { }

  public create(scope: Scope): ((search: string) => Observable<ISessionObject[]>) {
    return (search: string) => {
      if (!search.trim) {
        return EMPTY;
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
        new Pull(this.options.objectType, {
          name: 'results',
          predicate: and,
          sort: this.options.roleTypes.map((roleType: RoleType) => new Sort({ roleType })),
        }),
      ];

      return scope
        .load('Pull', new PullRequest({ pulls }))
        .pipe(map((loaded: Loaded) => {
          return loaded.collections.results;
        }));
    };
  }
}
