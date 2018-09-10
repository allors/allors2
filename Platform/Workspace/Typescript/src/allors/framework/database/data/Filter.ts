import { MetaObjectType, ObjectType } from '../../meta';
import { Predicate } from './Predicate';
import { Sort } from './Sort';

export class Filter {

  public objectType: ObjectType | MetaObjectType;

  public predicate: Predicate;

  public sort: Sort[];

  constructor(fields?: Partial<Filter> | MetaObjectType | ObjectType) {
    if ((fields as MetaObjectType)._objectType) {
      this.objectType = fields as any;
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {
    const metaObjectType = this.objectType as MetaObjectType;
    const objectType = metaObjectType._objectType ? metaObjectType._objectType : this.objectType as ObjectType;

    return {
      kind: 'Filter',
      objecttype: objectType.id,
      predicate: this.predicate,
      sorting: this.sort,
    };
  }
}
