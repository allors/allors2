import { ObjectTypeData } from '@allors/workspace/meta/system';
import { Lookup } from './utils/Lookup';
import { InternalMetaPopulation } from './internal/InternalMetaPopulation';
import { InternalClass } from './internal/InternalClass';
import { InternalComposite } from './internal/InternalComposite';
import { LazyComposite } from './LazyComposite';

export class LazyClass extends LazyComposite implements InternalClass {
  readonly isInterface = false;
  readonly isClass = true;
  readonly classes: Set<InternalClass>;

  constructor(metaPopulation: InternalMetaPopulation, data: ObjectTypeData, lookup: Lookup) {
    super(metaPopulation, data, lookup);
    this.classes = new Set([this]);
  }

  isAssignableFrom(objectType: InternalComposite): boolean {
    return this === objectType;
  }
}
