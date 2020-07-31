import { MetaPopulation } from './MetaPopulation';
import { AssociationType } from './AssociationType';
import { RoleType, RoleTypeVirtual } from './RoleType';
import { RelationTypeData } from './Data';
import { ObjectType } from './ObjectType';

export class RelationType {
  public id: string;
  public associationType: AssociationType;
  public roleType: RoleType;
  public isDerived: boolean;

  constructor(
    public metaPopulation: MetaPopulation,
    relationTypeData: RelationTypeData
  ) {
    this.id = relationTypeData.id;
    this.associationType = new AssociationType(this, relationTypeData.associationType);
    this.roleType = new RoleType(this, relationTypeData.roleType);
    this.isDerived = relationTypeData.isDerived ?? false;

    if (relationTypeData.concreteRoleTypes) {
      relationTypeData.concreteRoleTypes.forEach((dataConcreteRoleType) => {
        const roleTypeOverride = new RoleTypeVirtual();
        roleTypeOverride.isRequired = dataConcreteRoleType.isRequired;
        const objectType = this.metaPopulation.metaObjectById.get(
          dataConcreteRoleType.objectTypeId
        ) as ObjectType;

        this.roleType.overridesByClass.set(objectType, roleTypeOverride);
      });
    }

    this.associationType.objectType.exclusiveRoleTypes.push(this.roleType);
    this.roleType.objectType.exclusiveAssociationTypes.push(this.associationType);

    this.associationType.objectType.roleTypes.push(this.roleType);
    this.roleType.objectType.associationTypes.push(this.associationType);

    if (this.associationType.objectType.isInterface) {
      this.associationType.objectType.subtypes.forEach((subtype) =>
        subtype.roleTypes.push(this.roleType)
      );
    }

    if (this.roleType.objectType.isInterface) {
      this.roleType.objectType.subtypes.forEach((subtype) =>
        subtype.associationTypes.push(this.associationType)
      );
    }
  }
}
