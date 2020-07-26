import { PropertyType, ObjectType } from '../meta';
import { ISessionObject } from '../workspace/ISessionObject';
import { ParameterizablePredicate, ParameterizablePredicateArgs } from './ParameterizablePredicate';
import { Extent } from './Extent';
import { CompositeTypes } from '../workspace/Types';

export interface ContainedInArgs extends ParameterizablePredicateArgs, Pick<ContainedIn, 'propertyType' | 'extent' | 'objects'> {}

export class ContainedIn extends ParameterizablePredicate {
  propertyType: PropertyType;
  extent?: Extent;
  objects?: Array<CompositeTypes>;

  constructor(propertyType: PropertyType);
  constructor(args: ContainedInArgs);
  constructor(args: ContainedInArgs | PropertyType) {
    super();

    if ((args as PropertyType).objectType) {
      this.propertyType = args as PropertyType;
    } else if (args) {
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
