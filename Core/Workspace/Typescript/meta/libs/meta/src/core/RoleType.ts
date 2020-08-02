import { RoleType } from '@allors/workspace/meta';

const displayName = Symbol('displayName');

declare module '@allors/workspace/meta' {
  interface RoleType {
    displayName: string;
  }
}

Object.defineProperty(RoleType, 'displayName', {
  get(this: RoleType): string {
    return (this as any)[displayName] || this.name;
  },

  set(this: RoleType, value: string): void {
    (this as any)[displayName] = value;
  },
});
