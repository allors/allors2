import { domain } from '../domain';
import { AutomatedAgent } from '../generated/AutomatedAgent.g';

declare module '../generated/AutomatedAgent.g' {
    interface AutomatedAgent {
        displayName: string;

    }
}

domain.extend((workspace) => {

    const obj: AutomatedAgent = workspace.prototypeByName['AutomatedAgent'];

    Object.defineProperty(obj, 'displayName', {
        get(this: AutomatedAgent): string {
            if (this.UserName) {
                return this.UserName;
            }

            return 'N/A';
        },
    });
});
