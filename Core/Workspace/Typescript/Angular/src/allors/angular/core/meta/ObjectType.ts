import { FilterDefinition } from '../filter/FilterDefinition';
import { Filter } from '../filter/Filter';

declare module '../../../framework/meta/ObjectType' {
  interface ObjectType {
    filterDefinition?: FilterDefinition;
    filter?: Filter;
  }
}

