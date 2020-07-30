import { Sorter } from '../../core/sorting/Sorter';

declare module '../../../framework/meta/ObjectType' {
  interface ObjectType {
    sorter: Sorter;
  }
}
