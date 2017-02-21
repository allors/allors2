import { Person } from "../../generated/Person.g";

declare module "../../generated/Person.g" {
    interface Person {
        hello();

        displayName;
    }
}

Person.prototype.hello = function(this: Person) {
    return `Hello ${this.displayName()}`;
};

Person.prototype.displayName = function (this: Person) {
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
};
