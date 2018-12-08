import { ObjectTypeRef, ObjectType } from '../../meta';
import { Predicate } from './Predicate';
import { Sort } from './Sort';

export class Filter {

  public objectType: ObjectType | ObjectTypeRef;

  public predicate: Predicate;

  public sort: Sort[];

  constructor(fields?: Partial<Filter> | ObjectTypeRef | ObjectType) {
    if ((fields as ObjectTypeRef).objectType) {
      this.objectType = fields as ObjectTypeRef;
    } else if (fields instanceof ObjectType) {
      this.objectType = fields;
    } else {
      Object.assign(this, fields);
    }
  }

  public toJSON(): any {
    const metaObjectType = this.objectType as ObjectTypeRef;
    const objectType = metaObjectType.objectType ? metaObjectType.objectType : this.objectType as ObjectType;

    return {
      kind: 'Filter',
      objecttype: objectType.id,
      predicate: this.predicate,
      sorting: this.sort,
    };
  }
}
