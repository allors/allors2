import { ConcreteRoleType } from '../../framework/meta/ConcreteRoleType';

const displayName = Symbol('displayName');

declare module '../../framework/meta/ConcreteRoleType' {
    interface ConcreteRoleType {
        displayName;
    }
}

Object.defineProperty(ConcreteRoleType, 'displayName', {
    get(this: ConcreteRoleType): string {
        return this[displayName] || this.name;
    },

    set(this: ConcreteRoleType, value: string): void {
        this[displayName] = value;
    },
});
