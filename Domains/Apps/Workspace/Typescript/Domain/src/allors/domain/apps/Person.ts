import { Person } from "../generated/Person.g";

declare module "../generated/Person.g" {
    interface Person {
        displayName;
    }
}

Object.defineProperty(Person.prototype, "displayName", {
  get(this: Person): string {
    if (this.FirstName || this.LastName) {
        if (this.FirstName && this.LastName) {
            return this.FirstName + " " + this.LastName;
        } else if (this.LastName) {
            return this.LastName;
        } else {
            return this.FirstName;
        }
    }

    if (this.UserName) {
        return this.UserName;
    }

    return "N/A";
  },
});
