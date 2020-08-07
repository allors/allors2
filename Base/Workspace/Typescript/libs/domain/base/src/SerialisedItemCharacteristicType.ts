import { Meta } from '@allors/meta/generated';
import { assert } from '@allors/meta/system';
import { SerialisedItemCharacteristicType } from '@allors/domain/generated';

declare module '@allors/domain/generated' {
  interface SerialisedItemCharacteristicType {
    displayName: string;
  }
}

export function extendSerialisedItemCharacteristicType(workspace) {
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
}
