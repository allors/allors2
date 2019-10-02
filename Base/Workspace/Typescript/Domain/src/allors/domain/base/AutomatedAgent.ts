import { domain } from '../domain';
import { AutomatedAgent } from '../generated/AutomatedAgent.g';
import { Meta } from '../../meta/generated/domain.g';

declare module '../generated/AutomatedAgent.g' {
    interface AutomatedAgent {
        displayName: string;

    }
}

domain.extend((workspace) => {

    const m = workspace.metaPopulation as Meta;
    const obj = workspace.constructorByObjectType.get(m.AutomatedAgent).prototype as any;

    Object.defineProperty(obj, 'displayName', {
        configurable: true,
        get(this: AutomatedAgent): string {
            if (this.UserName) {
                return this.UserName;
            }

            return 'N/A';
        },
    });
});
