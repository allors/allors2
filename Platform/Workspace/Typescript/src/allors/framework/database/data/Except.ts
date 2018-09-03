import { Sort } from './Sort';
import { Extent } from './Extent';
import { ObjectType, MetaObjectType } from '../../meta';

export class Except {
  public operands: Extent[];

  public sort: Sort[];

  public get objectType(): ObjectType | MetaObjectType {
    return this.operands && this.operands.length > 0 ? this.operands[0].objectType : undefined;
  } 

  constructor(fields?: Partial<Except>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      kind: "Except",
      operands: this.operands,
      sorting: this.sort,
    };
  }
}
