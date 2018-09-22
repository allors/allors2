import { domain } from '../domain';
import { SerialisedInventoryItemCharacteristicType } from '../generated/SerialisedInventoryItemCharacteristicType.g';

declare module '../generated/SerialisedInventoryItemCharacteristicType.g' {
    interface SerialisedInventoryItemCharacteristicType {
        displayName;
    }
}

domain.extend((workspace) => {

    const obj: SerialisedInventoryItemCharacteristicType = workspace.prototypeByName['SerialisedInventoryItemCharacteristicType'];

    Object.defineProperties(obj, {
        displayName: {
            get(this: SerialisedInventoryItemCharacteristicType): string {
                return  this.UnitOfMeasure ?  this.Name + ' (' + this.UnitOfMeasure.Abbreviation + ')' : this.Name;
            },
        },
    });
});
