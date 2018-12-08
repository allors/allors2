import { Sort } from './Sort';
import { Extent } from './Extent';
import { ObjectType, ObjectTypeRef } from '../../meta';

export class Intersect {
  public operands: Extent[];

  public sort: Sort[];

  public get objectType(): ObjectType | ObjectTypeRef {
    return this.operands && this.operands.length > 0 ? this.operands[0].objectType : undefined;
  }

  constructor(fields?: Partial<Intersect>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      kind: 'Intersect',
      operands: this.operands,
      sorting: this.sort,
    };
  }
}
