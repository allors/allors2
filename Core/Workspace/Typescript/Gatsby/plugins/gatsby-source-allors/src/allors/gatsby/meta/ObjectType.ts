import { ObjectType, RoleType, AssociationType } from '../../framework';
import { ids } from '../../meta/generated/ids.g';

declare module '../../framework/meta/ObjectType' {
  interface ObjectType {
    gatsbyProperties: [{ name: string, type: "String" | "Data" | "Int" | "Float" | "Boolean" }];
    _isGatsby: boolean;
    _isUnion: boolean;
    _name: string;
    _roleTypes: RoleType[];
    _associationTypes: AssociationType[];
    gatsbyDerive(): void;
  }
}

ObjectType.prototype["gatsbyDerive"] = function (this: ObjectType) {

  if (!this._isGatsby) {
    delete (this._isUnion);
    delete (this._name);
    delete (this._roleTypes);
    delete (this._associationTypes)
    return;
  }

  if (this.isUnit) {
    switch (this.id) {

      case ids.Binary:
      case ids.Decimal:
      case ids.String:
      case ids.Unique:
        this._name = "String";
        break;

      case ids.Boolean:
        this._name = "Boolean";
        break;

      case ids.DateTime:
        this._name = "Date";
        break;

      case ids.Float:
        this._name = "Float";
        break;

      case ids.Integer:
        this._name = "Int";
        break;

      default:
        throw new Error("unknown unit type " + this.name)
    }
  } else {
    this._roleTypes = this.roleTypes.filter((v) => v.isGatsby);
    this._associationTypes = this.associationTypes.filter((v) => v.isGatsby);

    this._isUnion = this.isInterface && this.classes.length > 1;

    if (!this.classes || this.classes.length == 0) {
      console.log(this);
    }

    this._name = (this.isClass || this._isUnion) ?
      this._name = `Allors${this.name}` :
      `Allors${this.classes[0].name}`;

  }
}
