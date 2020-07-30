import { Filter, FilterDefinition } from '../filter';

declare module '../../../framework/meta/ObjectType' {
  interface ObjectType {
    filterDefinition?: FilterDefinition;
    filter?: Filter;
  }
}
