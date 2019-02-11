import { Exists } from '../../../../allors/framework';
import { FilterFieldDefinition } from './filterFieldDefinition';

export class FilterField {
  definition: FilterFieldDefinition;
  value: any;
  value2?: any;

  display: string;

  get argument() {
    let value = (this.value2 !== undefined && this.value2 !== null) ? [this.value, this.value2] : this.value;

    if (this.definition.isLike) {
      value = value + '%';
    }

    if (this.definition.isExists) {
      const exists = this.definition.predicate as Exists;
      value = value ? exists.propertyType.id : undefined;
    }

    return value;
  }

  constructor(fields?: Partial<FilterField>) {
    Object.assign(this, fields);
  }
}

