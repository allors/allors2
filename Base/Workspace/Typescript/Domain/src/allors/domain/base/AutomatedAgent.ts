import { domain } from '../domain';
import { AutomatedAgent } from '../generated/AutomatedAgent.g';
import { Meta } from '../../meta/generated/domain.g';
import { assert } from '../../framework';

declare module '../generated/AutomatedAgent.g' {
  interface AutomatedAgent {
    displayName: string;

  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.AutomatedAgent);
  assert(cls);

  Object.defineProperty(cls.prototype, 'displayName', {
    configurable: true,
    get(this: AutomatedAgent): string {
      return this.UserName ?? 'N/A';
    },
  });
});
