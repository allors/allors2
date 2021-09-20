import { Origin, pluralize, ObjectTypeData } from '@allors/workspace/meta/system';
import { frozenEmptySet } from './utils/frozenEmptySet';
import { InternalComposite } from './internal/InternalComposite';
import { InternalInterface } from './internal/InternalInterface';
import { InternalAssociationType } from './internal/InternalAssociationType';
import { InternalRoleType } from './internal/InternalRoleType';
import { InternalMethodType } from './internal/InternalMethodType';
import { InternalClass } from './internal/InternalClass';
import { InternalMetaPopulation } from './internal/InternalMetaPopulation';
import { LazyRelationType } from './LazyRelationType';
import { LazyMethodType } from './LazyMethodType';
import { Lookup } from './utils/Lookup';

export abstract class LazyComposite implements InternalComposite {
  isUnit = false;
  isComposite = true;
  readonly tag: number;
  readonly singularName: string;
  readonly origin: Origin;

  associationTypes!: Set<InternalAssociationType>;
  roleTypes!: Set<InternalRoleType>;
  methodTypes!: Set<InternalMethodType>;

  directSupertypes!: Set<InternalInterface>;
  supertypes!: Set<InternalInterface>;

  directAssociationTypes: Set<InternalAssociationType> = new Set();
  directRoleTypes: Set<InternalRoleType> = new Set();
  directMethodTypes: Set<InternalMethodType> = new Set();

  abstract isInterface: boolean;
  abstract isClass: boolean;
  abstract classes: Set<InternalClass>;

  private _pluralName?: string;

  get pluralName() {
    return (this._pluralName ??= pluralize(this.singularName));
  }

  abstract isAssignableFrom(objectType: InternalComposite): boolean;

  constructor(public metaPopulation: InternalMetaPopulation, public d: ObjectTypeData, lookup: Lookup) {
    const [t, s] = this.d;
    this.tag = t;
    this.singularName = s;
    this.origin = lookup.o.get(t) ?? Origin.Database;
    metaPopulation.onNewComposite(this);
  }
  onNewAssociationType(associationType: InternalAssociationType) {
    this.directAssociationTypes.add(associationType);
  }

  onNewRoleType(roleType: InternalRoleType) {
    this.directRoleTypes.add(roleType);
  }

  derive(lookup: Lookup): void {
    const [, , d, r, m, p] = this.d;

    this._pluralName = p;

    if (d) {
      this.directSupertypes = new Set(d?.map((v) => this.metaPopulation.metaObjectByTag.get(v) as InternalInterface));
    } else {
      this.directSupertypes = frozenEmptySet as Set<InternalInterface>;
    }

    r?.forEach((v) => new LazyRelationType(this, v, lookup));

    if (m) {
      this.directMethodTypes = new Set(m?.map((v) => new LazyMethodType(this, v)));
    } else {
      this.directMethodTypes = frozenEmptySet as Set<InternalMethodType>;
    }
  }

  deriveSuper(): void {
    if (this.directSupertypes.size > 0) {
      this.supertypes = new Set(this.supertypeGenerator());
    } else {
      this.supertypes = frozenEmptySet as Set<InternalInterface>;
    }
  }

  deriveOperand() {
    this.associationTypes = new Set(this.associationTypeGenerator());
    this.roleTypes = new Set(this.roleTypeGenerator());
    this.methodTypes = new Set(this.methodTypeGenerator());

    this.associationTypes.forEach((v) => ((this as Record<string, unknown>)[v.name] = v));
    this.roleTypes.forEach((v) => ((this as Record<string, unknown>)[v.name] = v));
    this.methodTypes.forEach((v) => ((this as Record<string, unknown>)[v.name] = v));
  }

  *supertypeGenerator(): IterableIterator<InternalInterface> {
    if (this.supertypes) {
      yield* this.supertypes.values();
    } else {
      for (const supertype of this.directSupertypes.values()) {
        yield supertype;
        yield* supertype.supertypeGenerator();
      }
    }
  }

  *associationTypeGenerator(): IterableIterator<InternalAssociationType> {
    if (this.associationTypes) {
      yield* this.associationTypes;
    } else {
      yield* this.directAssociationTypes;
      for (const supertype of this.directSupertypes) {
        yield* supertype.associationTypeGenerator();
      }
    }
  }

  *roleTypeGenerator(): IterableIterator<InternalRoleType> {
    if (this.roleTypes) {
      yield* this.roleTypes;
    } else {
      yield* this.directRoleTypes;
      for (const supertype of this.directSupertypes) {
        yield* supertype.roleTypeGenerator();
      }
    }
  }

  *methodTypeGenerator(): IterableIterator<InternalMethodType> {
    if (this.methodTypes) {
      yield* this.methodTypes;
    } else {
      yield* this.directMethodTypes;
      for (const supertype of this.directSupertypes) {
        yield* supertype.methodTypeGenerator();
      }
    }
  }
}
