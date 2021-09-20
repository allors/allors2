import { ObjectTypeData } from '@allors/workspace/meta/system';
import { Lookup } from './utils/Lookup';
import { InternalClass } from './internal/InternalClass';
import { InternalComposite } from './internal/InternalComposite';
import { InternalInterface } from './internal/InternalInterface';
import { InternalMetaPopulation } from './internal/InternalMetaPopulation';
import { LazyComposite } from './LazyComposite';

export class LazyInterface extends LazyComposite implements InternalInterface {
  readonly isInterface = true;
  readonly isClass = false;

  subtypes: Set<InternalComposite>;
  classes: Set<InternalClass>;

  constructor(metaPopulation: InternalMetaPopulation, data: ObjectTypeData, lookup: Lookup) {
    super(metaPopulation, data, lookup);
    this.subtypes = new Set();
    this.classes = new Set();
  }

  deriveSub(): void {
    this.metaPopulation.composites.forEach((v) => {
      if (v.supertypes.has(this)) {
        this.subtypes.add(v as InternalComposite);
        if (v.isClass) {
          this.classes.add(v as InternalClass);
        }
      }
    });
  }

  isAssignableFrom(objectType: InternalComposite): boolean {
    return this === objectType || this.subtypes.has(objectType);
  }
}
