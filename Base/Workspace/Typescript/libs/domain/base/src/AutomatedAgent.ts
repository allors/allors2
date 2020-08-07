import { Meta } from '@allors/meta/generated';
import { AutomatedAgent } from '@allors/domain/generated';
import { assert } from '@allors/meta/system';

declare module '@allors/domain/generated' {
  interface AutomatedAgent {
    displayName: string;

  }
}

export function extendAutomatedAgent(workspace) {

  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.AutomatedAgent);
  assert(cls);

  Object.defineProperty(cls.prototype, 'displayName', {
    configurable: true,
    get(this: AutomatedAgent): string {
      return this.UserName ?? 'N/A';
    },
  });
};
