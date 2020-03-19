import { ObjectType } from '../../framework/meta/ObjectType';

const icon = Symbol('icon');
const displayName = Symbol('displayName');

declare module '../../framework/meta/ObjectType' {
  interface ObjectType {
    icon: string;
    displayName: string;
    list: string;
    overview: string;
  }
}

function humanize(input: string): string {
  let result = input && input
    .replace(/([a-z\d])([A-Z])/g, '$1 $2')
    .replace(/([A-Z]+)([A-Z][a-z\d]+)/g, '$1 $2');

  result = result && result.charAt(0).toUpperCase() + result.slice(1);

  return result;
}

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
