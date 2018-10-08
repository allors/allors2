import { Like, ParametrizedPredicate } from 'src/allors/framework';

export class FilterField {
  predicate: ParametrizedPredicate;
  value: any;
  value2?: any;

  constructor(fields?: Partial<FilterField>) {
    Object.assign(this, fields);
  }
}

