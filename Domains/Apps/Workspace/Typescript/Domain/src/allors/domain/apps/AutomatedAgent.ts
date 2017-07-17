import { AutomatedAgent } from '../generated/AutomatedAgent.g';

declare module '../generated/AutomatedAgent.g' {
    interface AutomatedAgent {
        displayName: string;

    }
}

Object.defineProperty(AutomatedAgent.prototype, 'displayName', {
  get: function (this: AutomatedAgent): string {
    if (this.UserName) {
        return this.UserName;
    }

    return 'N/A';
  },
});
