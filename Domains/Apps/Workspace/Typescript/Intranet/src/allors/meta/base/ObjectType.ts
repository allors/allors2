import { ObjectType } from '../../framework/meta/ObjectType';

const icon = Symbol('icon');
const displayName = Symbol('displayName');

declare module '../../framework/meta/ObjectType' {
    interface ObjectType {
        icon;
        displayName;
    }
}

const x = ObjectType;

Object.defineProperty(ObjectType.prototype, 'icon', {
    get(this: ObjectType): string {
        return this[icon];
    },

    set(this: ObjectType, value: string): void {
        this[icon] = value;
    },
});

Object.defineProperty(ObjectType.prototype, 'displayName', {
    get(this: ObjectType): string {
        return this[displayName] || this.name;
    },

    set(this: ObjectType, value: string): void {
        this[displayName] = value;
    },
});
