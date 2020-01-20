import { ObjectType } from '../../framework/meta/ObjectType';
import { MetaGatsby } from './MetaGatsby';

const metaGatsby = 'metaGatsby';
const metaGatsbySymbol = Symbol(metaGatsby);

declare module '../../framework/meta/ObjectType' {
    interface ObjectType {
        metaGatsby: MetaGatsby;
    }
}

Object.defineProperty(ObjectType.prototype, 'metaGatsby', {
    get(this: ObjectType): MetaGatsby {
        return this[metaGatsbySymbol];
    },

    set(this: ObjectType, value: MetaGatsby): void {
        this[metaGatsbySymbol] = value;
    },
});
