import { ObjectType } from '../../framework/meta/ObjectType';
import { humanize } from '../../angular/core/humanize';

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
        return this[displayName] || humanize(this.name);
    },

    set(this: ObjectType, value: string): void {
        this[displayName] = value;
    },
});
