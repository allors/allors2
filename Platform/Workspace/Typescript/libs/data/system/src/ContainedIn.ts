import { ObjectType, PropertyType } from '@allors/meta/system';
import { ISessionObject, CompositeTypes } from '@allors/domain/system';

import { ParameterizablePredicateArgs, ParameterizablePredicate } from './ParameterizablePredicate';
import { IExtent } from './IExtent';

export interface ContainedInArgs extends ParameterizablePredicateArgs, Pick<ContainedIn, 'propertyType' | 'extent' | 'objects'> {}

export class ContainedIn extends ParameterizablePredicate {
  propertyType: PropertyType;
  extent?: IExtent;
  objects?: Array<CompositeTypes>;

  constructor(propertyType: PropertyType);
  constructor(args: ContainedInArgs);
  constructor(args: ContainedInArgs | PropertyType) {
    super();

    if ((args as PropertyType).objectType) {
      this.propertyType = args as PropertyType;
    } else {
      Object.assign(this, args);
      this.propertyType = (args as ContainedInArgs).propertyType;
    }
  }

  get objectType(): ObjectType {
    return this.propertyType.objectType;
  }

  public toJSON(): any {
    return {
      kind: 'ContainedIn',
      dependencies: this.dependencies,
      propertytype: this.propertyType.id,
      parameter: this.parameter,
      extent: this.extent,
      objects: this.objects ? this.objects.map((v) => ((v as ISessionObject).id ? (v as ISessionObject).id : v)) : undefined,
    };
  }
}
