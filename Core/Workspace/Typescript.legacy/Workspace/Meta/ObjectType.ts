namespace Allors.Meta {
    export class ObjectType {
        name: string;
        roleTypeByName: { [name: string]: RoleType; } = {};

        constructor(data: Data.ObjectType) {
            this.name = data.name;
            _.forEach(data.roleTypes, roleTypeData => {
                var roleType = new RoleType(roleTypeData);
                this.roleTypeByName[roleType.name] = roleType;
            });
        }
    }
}