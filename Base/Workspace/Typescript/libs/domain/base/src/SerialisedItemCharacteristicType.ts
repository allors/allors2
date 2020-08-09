import { Meta } from '@allors/meta/generated';
import { assert } from '@allors/meta/system';
import { SerialisedItemCharacteristicType } from '@allors/domain/generated';
import { Workspace } from '@allors/domain/system';

export function extendSerialisedItemCharacteristicType(workspace: Workspace) {
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
