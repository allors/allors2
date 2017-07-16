import { Person } from '../generated/Person.g';

declare module '../generated/Person.g' {
    interface Person {
        displayName: string;

    }
}

Object.defineProperty(Person.prototype, 'displayName', {
  get: function (this: Person): string {
    if (this.FirstName || this.LastName) {
        if (this.FirstName && this.LastName) {
            return this.FirstName + ' ' + this.LastName;
        } else if (this.LastName) {
            return this.LastName;
        } else {
            return this.FirstName;
        }
    }

    if (this.UserName) {
        return this.UserName;
    }

    return 'N/A';
  },
});
