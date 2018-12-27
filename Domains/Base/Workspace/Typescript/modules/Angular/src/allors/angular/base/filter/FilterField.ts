import { FilterFieldDefinition } from './filterFieldDefinition';

export class FilterField {
  definition: FilterFieldDefinition;
  value: any;
  value2?: any;

  constructor(fields?: Partial<FilterField>) {
    Object.assign(this, fields);
  }
}

