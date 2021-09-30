import { Multiplicity, Origin, RelationTypeData } from '@allors/workspace/meta/system';
import { InternalAssociationType } from './internal/InternalAssociationType';
import { InternalComposite } from './internal/InternalComposite';
import { InternalMetaPopulation } from './internal/InternalMetaPopulation';
import { InternalObjectType } from './internal/InternalObjectType';
import { InternalRelationType } from './internal/InternalRelationType';
import { InternalRoleType } from './internal/InternalRoleType';
import { LazyRoleType } from './LazyRoleType';
import { Lookup } from './utils/Lookup';

export class LazyRelationType implements InternalRelationType {
  readonly metaPopulation: InternalMetaPopulation;

  readonly tag: number;
  readonly multiplicity: Multiplicity;
  readonly origin: Origin;
  readonly isDerived: boolean;

  readonly associationType: InternalAssociationType;
  readonly roleType: InternalRoleType;

  constructor(associationObjectType: InternalComposite, data: RelationTypeData, lookup: Lookup) {
    this.metaPopulation = associationObjectType.metaPopulation as InternalMetaPopulation;

    const [t, r] = data;
    const roleObjectType = this.metaPopulation.metaObjectByTag.get(r) as InternalObjectType;

    this.tag = t;
    this.multiplicity = roleObjectType.isUnit ? Multiplicity.OneToOne : lookup.m.get(t) ?? Multiplicity.ManyToOne;
    this.origin = lookup.o.get(t) ?? Origin.Database;
    this.isDerived = lookup.d.has(t);

    this.metaPopulation.onNew(this);

    this.roleType = new LazyRoleType(this, associationObjectType, roleObjectType, this.multiplicity, data, lookup);
    this.associationType = this.roleType.associationType;

    if (this.roleType.objectType.isComposite) {
      (this.roleType.objectType as InternalComposite).onNewAssociationType(this.associationType);
    }

    (this.associationType.objectType as InternalComposite).onNewRoleType(this.roleType);
  }
}
