import { domain } from '../domain';
import { SerialisedItemCharacteristicType } from '../generated/SerialisedItemCharacteristicType.g';

declare module '../generated/SerialisedItemCharacteristicType.g' {
    interface SerialisedItemCharacteristicType {
        displayName;
    }
}

domain.extend((workspace) => {

    const obj: SerialisedItemCharacteristicType = workspace.prototypeByName['SerialisedItemCharacteristicType'];

    Object.defineProperties(obj, {
        displayName: {
            configurable: true,
            get(this: SerialisedItemCharacteristicType): string {
                return  this.UnitOfMeasure ?  this.Name + ' (' + this.UnitOfMeasure.Abbreviation + ')' : this.Name;
            },
        },
    });
});
