import { Sorter } from '../../core/sorting';

declare module '../../../framework/meta/ObjectType' {
  interface ObjectType {
    sorter: Sorter;
  }
}
