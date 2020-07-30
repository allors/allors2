import { Sorter } from '../sorting/Sorter';

declare module '../../../framework/meta/ObjectType' {
  interface ObjectType {
    sorter: Sorter;
  }
}
