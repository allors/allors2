/// <reference path="../../generated/domain/Person.g.ts" />
/// <reference path="../../Core/Workspace/Domain/Domain.ts" />
namespace Allors.Domain
{
    export interface Person {
        displayName: string;

        toString(): string;
    }

    domain.extend((workspace) => {

      const m = workspace.metaPopulation as Meta.Meta;
      const person = workspace.constructorByObjectType.get(m.Person).prototype as any;

      person.toString = function (this: Person) {
          return `Hello ${this.displayName}`;
      };

      Object.defineProperty(person, 'displayName', {
        get(this: Person): string {
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
    });
}
