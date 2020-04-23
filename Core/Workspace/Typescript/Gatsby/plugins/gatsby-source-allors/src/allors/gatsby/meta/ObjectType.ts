import { ObjectType, RoleType, AssociationType } from '../../framework';
import { ids } from '../../meta/generated/ids.g';

declare module '../../framework/meta/ObjectType' {
    interface ObjectType {
        gatsbyProperties: { name: string; type: 'String' | 'Data' | 'Int' | 'Float' | 'Boolean' }[];

        gatsbyRoleTypes: RoleType[];

        gatsbyAssociationTypes: AssociationType[];

        _isGatsby: boolean;
        _name: string;
        gatsbyDerive(): void;
    }
}

ObjectType.prototype['gatsbyDerive'] = function (this: ObjectType) {
    if (!this._isGatsby) {
        delete this._name;
        return;
    }

    if (this.isUnit) {
        switch (this.id) {
            case ids.Binary:
            case ids.Decimal:
            case ids.String:
            case ids.Unique:
                this._name = 'String';
                break;

            case ids.Boolean:
                this._name = 'Boolean';
                break;

            case ids.DateTime:
                this._name = 'Date';
                break;

            case ids.Float:
                this._name = 'Float';
                break;

            case ids.Integer:
                this._name = 'Int';
                break;

            default:
                throw new Error('unknown unit type ' + this.name);
        }
    } else {
        this._name = `Allors${this.name}`;
    }
};
