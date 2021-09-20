import { Origin, UnitTags } from '@allors/workspace/meta/system';
import { InternalUnit } from './internal/InternalUnit';
import { InternalMetaPopulation } from './internal/InternalMetaPopulation';

export class LazyUnit implements InternalUnit {
  isUnit = true;
  isComposite = false;
  isInterface = false;
  isClass = false;
  pluralName: string;
  origin = Origin.Database;
  isBinary = this.tag === UnitTags.Binary;
  isBoolean = this.tag === UnitTags.Boolean;
  isDecimal = this.tag === UnitTags.Decimal;
  isDateTime = this.tag === UnitTags.DateTime;
  isFloat = this.tag === UnitTags.Float;
  isInteger = this.tag === UnitTags.Integer;
  isString = this.tag === UnitTags.String;
  isUnique = this.tag === UnitTags.Unique;

  constructor(public metaPopulation: InternalMetaPopulation, public tag: number, public singularName: string) {
    this.pluralName = singularName === 'Binary' ? 'Binaries' : singularName + 's';
    metaPopulation.onNewObjectType(this);
  }
}
