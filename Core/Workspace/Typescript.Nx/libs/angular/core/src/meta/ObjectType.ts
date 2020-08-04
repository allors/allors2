import { Filter } from '../filter/Filter';
import { FilterDefinition } from '../filter/FilterDefinition';

declare module '@allors/meta/system' {
  interface ObjectType {
    filterDefinition?: FilterDefinition;
    filter?: Filter;
  }
}
