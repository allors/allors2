import { MetaObjectType, ObjectType } from '../../meta';
import { Extent } from './Extent';

export class PullArgument {

  public name: string;

  public value: string;

  constructor(fields?: Partial<PullArgument>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      name: this.name,
      value: this.value
    };
  }
}
