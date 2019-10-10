/// <reference path="../../../Client/generated/domain/Person.g.ts" />
/// <reference path="../../Core/Workspace/Domain/extend.ts" />
namespace Allors.Domain
{
    export interface Person {
        Fullname: string;

        toString(): string;
    }

    extend(Person,
    {
        get Fullname(): string {
            return this.LastName + (this.FirstName ? ", " : "") + this.FirstName;
        },

        set Fullname(value: string) {
            // Should be present, otherwise typing to fast in a search box causes an error
        },

        toString(): string {
            return this.FirstName;
        }
    });
}
