import { AssociationType,  } from '../../framework';

declare module '../../framework/meta/AssociationType' {
  interface AssociationType {
    isGatsby: boolean;
  }
}
