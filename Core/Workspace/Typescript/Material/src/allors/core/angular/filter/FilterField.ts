import { FilterFieldDefinition } from './FilterFieldDefinition';

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

    return value;
  }

  constructor(fields?: Partial<FilterField>) {
    Object.assign(this, fields);
  }
}

