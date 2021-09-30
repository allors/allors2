import { Origin, pluralize, Multiplicity, RelationTypeData } from '@allors/workspace/meta/system';
import { InternalAssociationType } from './internal/InternalAssociationType';
import { InternalObjectType } from './internal/InternalObjectType';
import { InternalRelationType } from './internal/InternalRelationType';
import { InternalRoleType } from './internal/InternalRoleType';
import { InternalUnit } from './internal/InternalUnit';
import { LazyAssociationType } from './LazyAssociationType';
import { InternalComposite } from './internal/InternalComposite';
import { Lookup } from './utils/Lookup';

export class LazyRoleType implements InternalRoleType {
  readonly objectType: InternalObjectType;
  readonly isOne: boolean;
  readonly isMany: boolean;
  readonly origin: Origin;
  readonly name: string;
  readonly singularName: string;
  readonly isDerived: boolean;
  readonly isRequired: boolean;
  readonly isUnique: boolean;
  readonly size?: number;
  readonly precision?: number;
  readonly scale?: number;
  readonly mediaType?: string;
  readonly operandTag: number;

  readonly associationType: InternalAssociationType;

  private _pluralName?: string;

  constructor(public relationType: InternalRelationType, associationObjectType: InternalComposite, roleObjectType: InternalObjectType, multiplicity: Multiplicity, data: RelationTypeData, lookup: Lookup) {
    this.isOne = (multiplicity & 2) == 0;
    this.isMany = !this.isOne;
    this.origin = relationType.origin;
    this.operandTag = relationType.tag;
    this.objectType = roleObjectType;

    this.isDerived = lookup.d.has(this.relationType.tag);
    this.isRequired = lookup.r.has(this.relationType.tag);
    this.isUnique = lookup.u.has(this.relationType.tag);
    this.mediaType = lookup.t.get(this.relationType.tag);

    const [, , v0, v1, v2, v3] = data;

    this.singularName = (!Number.isInteger(v0) ? (v0 as string) : undefined) ?? this.objectType.singularName;
    this._pluralName = !Number.isInteger(v1) ? (v1 as string) : undefined;

    if (this.objectType.isUnit) {
      const unit = this.objectType as InternalUnit;
      if (unit.isString || unit.isDecimal) {
        let sizeOrScale = undefined;
        let precision = undefined;

        if (Number.isInteger(v0)) {
          sizeOrScale = v0 as number;
          precision = v1 as number;
        } else if (Number.isInteger(v1)) {
          sizeOrScale = v1 as number;
          precision = v2 as number;
        } else {
          sizeOrScale = v2 as number;
          precision = v3 as number;
        }

        if (unit.isString) {
          this.size = sizeOrScale;
        }
        if (unit.isDecimal) {
          this.scale = sizeOrScale;
          this.precision = precision;
        }
      }
    }

    this.name = this.isOne ? this.singularName : this.pluralName;

    this.associationType = new LazyAssociationType(this, associationObjectType, multiplicity);
  }

  get pluralName() {
    return (this._pluralName ??= pluralize(this.singularName));
  }
}
