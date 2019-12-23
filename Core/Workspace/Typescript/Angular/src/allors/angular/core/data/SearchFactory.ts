import { Observable, EMPTY } from 'rxjs';
import { map } from 'rxjs/operators';

import { And, ISessionObject, Like, Or, PullRequest, Pull, RoleType, Sort } from '../../../framework';
import { Loaded, Context, ContextService } from '../framework';

import { SearchOptions } from './SearchOptions';

export class SearchFactory {
  constructor(private options: SearchOptions) { }

  public create(contextOrService: Context | ContextService): ((search: string) => Observable<ISessionObject[]>) {
    return (search: string) => {
      if (!search.trim) {
        return EMPTY;
      }

      const terms: string[] = search.trim().split(' ');

      const and: And = new And();

      if (this.options.predicates) {
        this.options.predicates.forEach((predicate) => {
          and.operands.push(predicate);
        });
      }

      terms.forEach((term: string) => {
        this.options.roleTypes.forEach((roleType: RoleType) => {
          and.operands.push(new Like({ roleType, value: '%' + term + '%' }));
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

      const context = contextOrService instanceof Context ? contextOrService : contextOrService.context;

      return context
        .load(new PullRequest({ pulls }))
        .pipe(map((loaded: Loaded) => {
          return loaded.collections.results;
        }));
    };
  }
}
