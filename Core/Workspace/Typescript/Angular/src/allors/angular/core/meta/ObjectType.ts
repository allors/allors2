import { FilterBuilder } from '../../core/filter';

declare module '../../../framework/meta/ObjectType' {
  interface ObjectType {
    filterBuilder: FilterBuilder;
  }
}

