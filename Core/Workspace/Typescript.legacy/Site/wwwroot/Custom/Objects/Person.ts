/// <reference path="../../generated/domain/Person.g.ts" />
/// <reference path="../../Core/Workspace/Domain/extend.ts" />
namespace Allors.Domain {
  export interface Person {
    displayName: string;

    toString(): string;
  }

  extend(Person, {
    toString(): string {
      return `Hello ${this.displayName}`;
    },
    get displayName(): string {
      const self = this as Allors.Domain.Person;

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
}
