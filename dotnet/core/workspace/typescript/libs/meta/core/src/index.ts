declare module '@allors/meta/system' {
  interface ObjectType {
    icon: string;
    displayName: string;
    list: string;
    overview: string;
  }

  interface RoleType {
    displayName: string;
  }
}

import './ObjectType';
import './RoleType';
