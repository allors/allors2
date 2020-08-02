import { ObjectType } from '@allors/workspace/meta';

import { Predicate } from './Predicate';
import { Sort } from './Sort';

export type ExtentArgs = Pick<Extent, 'objectType' | 'predicate' | 'sort'>;

export class Extent {
  public objectType: ObjectType;

  public predicate?: Predicate;

  public sort?: Sort[];

  constructor(objectType: ObjectType);
  constructor(args: ExtentArgs);
  constructor(args: ExtentArgs | ObjectType) {
    if (args instanceof ObjectType) {
      this.objectType = args;
    } else {
      Object.assign(this, args);
      this.objectType = args.objectType;
    }
  }

  public toJSON(): any {
    return {
      kind: 'Extent',
      objecttype: this.objectType.id,
      predicate: this.predicate,
      sorting: this.sort,
    };
  }
}
