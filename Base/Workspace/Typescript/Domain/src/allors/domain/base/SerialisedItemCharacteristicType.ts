import { domain } from '../domain';
import { SerialisedItemCharacteristicType } from '../generated/SerialisedItemCharacteristicType.g';
import { Meta } from '../../meta/generated/domain.g';

declare module '../generated/SerialisedItemCharacteristicType.g' {
    interface SerialisedItemCharacteristicType {
        displayName;
    }
}

domain.extend((workspace) => {

    const m = workspace.metaPopulation as Meta;
    const obj = workspace.constructorByObjectType.get(m.SerialisedItemCharacteristicType).prototype as any;

    Object.defineProperties(obj, {
        displayName: {
            configurable: true,
            get(this: SerialisedItemCharacteristicType): string {
                return  this.UnitOfMeasure ?  this.Name + ' (' + this.UnitOfMeasure.Abbreviation + ')' : this.Name;
            },
        },
    });
});
