import { ObjectType, humanize } from '@allors/meta/system';

const icon = Symbol('icon');
const displayName = Symbol('displayName');

Object.defineProperty(ObjectType.prototype, 'icon', {
  get(this: ObjectType): string {
    return (this as any)[icon];
  },

  set(this: ObjectType, value: string): void {
    (this as any)[icon] = value;
  },
});

Object.defineProperty(ObjectType.prototype, 'displayName', {
  get(this: ObjectType): string {
    return (this as any)[displayName] || humanize(this.name);
  },

  set(this: ObjectType, value: string): void {
    (this as any)[displayName] = value;
  },
});
