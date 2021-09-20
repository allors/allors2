import { RoleType } from '@allors/meta/system';

const displayName = Symbol('displayName');

Object.defineProperty(RoleType, 'displayName', {
  get(this: RoleType): string {
    return (this as any)[displayName] || this.name;
  },

  set(this: RoleType, value: string): void {
    (this as any)[displayName] = value;
  },
});
