import { RoleType,  } from '../../framework';

declare module '../../framework/meta/RoleType' {
  interface RoleType {
    isGatsby: boolean;
  }
}
