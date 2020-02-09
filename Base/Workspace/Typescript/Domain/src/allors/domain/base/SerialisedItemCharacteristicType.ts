import { domain } from '../domain';
import { SerialisedItemCharacteristicType } from '../generated/SerialisedItemCharacteristicType.g';
import { Meta } from '../../meta/generated/domain.g';
import { assert } from '../../framework';

declare module '../generated/SerialisedItemCharacteristicType.g' {
  interface SerialisedItemCharacteristicType {
    displayName: string;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.SerialisedItemCharacteristicType);
  assert(cls);

  Object.defineProperties(cls.prototype, {
    displayName: {
      configurable: true,
      get(this: SerialisedItemCharacteristicType): string {
        return (this.UnitOfMeasure ? this.Name + ' (' + this.UnitOfMeasure.Abbreviation + ')' : this.Name) ?? '';
      },
    },
  });
});
