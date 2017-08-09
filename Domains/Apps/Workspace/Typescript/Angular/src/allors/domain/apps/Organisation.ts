import { Organisation } from '../generated/Organisation.g';

declare module '../generated/Organisation.g' {
    interface Organisation {
        displayName: string;

    }
}

Object.defineProperty(Organisation.prototype, 'displayName', {
  get: function (this: Organisation): string {

    if (this.Name) {
        return this.Name;
    }

    return 'N/A';
  },
});
