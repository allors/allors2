import { RoleType } from '../../framework/meta/RoleType';

const displayName = Symbol('displayName');

declare module '../../framework/meta/RoleType' {
    interface RoleType {
        displayName;
    }
}

Object.defineProperty(RoleType, 'displayName', {
    get(this: RoleType): string {
        return this[displayName] || this.name;
    },

    set(this: RoleType, value: string): void {
        this[displayName] = value;
    },
});
