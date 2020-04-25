import { PropertyType, ObjectType } from '../meta';
import { ISessionObject } from '../workspace/SessionObject';
import { ParametrizedPredicate } from './ParametrizedPredicate';
import { Extent } from './Extent';
import { CompositeTypes } from '../workspace/Types';

export class ContainedIn extends ParametrizedPredicate {
  public propertyType: PropertyType;
  public extent: Extent;
  public objects: Array<CompositeTypes>;

  constructor(fields?: Partial<ContainedIn> | PropertyType) {
    super();

    if ((fields as PropertyType).objectType) {
      this.propertyType = fields as PropertyType;
    } else {
      Object.assign(this, fields);
    }
  }

  get objectType(): ObjectType {
    return this.propertyType.objectType;
  }

  public toJSON(): any {
    return {
      kind: 'ContainedIn',
      propertytype: this.propertyType.id,
      parameter: this.parameter,
      extent: this.extent,
      objects: this.objects ? this.objects.map((v) => ((v as ISessionObject).id ? (v as ISessionObject).id : v)) : undefined,
    };
  }
}
