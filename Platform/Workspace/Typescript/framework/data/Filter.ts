import { ObjectType } from '../meta';
import { Predicate } from './Predicate';
import { Sort } from './Sort';

export type FilterArgs = Pick<Filter, 'objectType' | 'predicate' | 'sort'>;

export class Filter {
  public objectType: ObjectType;

  public predicate?: Predicate;

  public sort?: Sort[];

  constructor(objectType: ObjectType);
  constructor(args: FilterArgs);
  constructor(args: FilterArgs | ObjectType) {
    if (args instanceof ObjectType) {
      this.objectType = args;
    } else if (args) {
      Object.assign(this, args);
      this.objectType = args.objectType;
    }
  }

  public toJSON(): any {
    return {
      kind: 'Filter',
      objecttype: this.objectType.id,
      predicate: this.predicate,
      sorting: this.sort,
    };
  }
}
