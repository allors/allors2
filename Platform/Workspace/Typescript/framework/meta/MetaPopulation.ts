import { MetaData } from './Data';

import { MetaObject } from './MetaObject';
import { Kind, ObjectType } from './ObjectType';
import { unitIdByTypeName } from './Units';
import { RelationType } from './RelationType';
import { MethodType } from './MethodType';
import { RoleType, RoleTypeVirtual } from './RoleType';
import { AssociationType } from './AssociationType';

export class MetaPopulation {
  readonly metaObjectById: Map<string, MetaObject>;
  readonly objectTypeByName: Map<string, ObjectType>;

  readonly units: ObjectType[];
  readonly composites: ObjectType[];
  readonly interfaces: ObjectType[];
  readonly classes: ObjectType[];
  readonly objectTypes: ObjectType[];
  readonly relationTypes: RelationType[];
  readonly methodTypes: MethodType[];

  constructor(data: MetaData) {

    this.metaObjectById = new Map();
    this.objectTypeByName = new Map();

    this.composites = [];
    this.objectTypes = [];

    // Units
    this.units = ['Binary', 'Boolean', 'DateTime', 'Decimal', 'Float', 'Integer', 'String', 'Unique']
      .map((unitName) => {
        const objectType = new ObjectType(this);
        objectType.id = unitIdByTypeName[unitName];
        objectType.name = unitName;
        objectType.plural = unitName === 'Binary' ? 'Binaries' : unitName + 's';
        objectType.kind = Kind.unit;
        this.objectTypes.push(objectType);
        this.objectTypeByName.set(objectType.name, objectType);
        this.metaObjectById.set(objectType.id, objectType);
        return objectType;
      });

    // Interfaces
    this.interfaces = data.interfaces.map((interfaceData) => {
      const objectType = new ObjectType(this);
      objectType.id = interfaceData.id;
      objectType.name = interfaceData.name;
      objectType.plural = interfaceData.plural;
      objectType.kind = Kind.interface;
      this.composites.push(objectType);
      this.objectTypes.push(objectType);
      this.objectTypeByName.set(objectType.name, objectType);
      this.metaObjectById.set(objectType.id, objectType);
      return objectType;
    });

    // Classes
    this.classes = data.classes.map((classData) => {
      const objectType = new ObjectType(this);
      objectType.id = classData.id;
      objectType.name = classData.name;
      objectType.plural = classData.plural;
      objectType.kind = Kind.class;
      this.composites.push(objectType);
      this.objectTypes.push(objectType);
      this.objectTypeByName.set(objectType.name, objectType);
      this.metaObjectById.set(objectType.id, objectType);
      return objectType;
    });

    const dataObjectTypes = [].concat(data.interfaces).concat(data.classes);

    // Create Type Hierarchy
    dataObjectTypes.forEach((dataObjectType) => {
      const objectType = this.metaObjectById.get(dataObjectType.id) as ObjectType;
      objectType.interfaces = dataObjectType.interfaceIds ? dataObjectType.interfaceIds.map((v) => this.metaObjectById.get(v) as ObjectType) : [];
    });

    // Derive subtypes
    this.composites
      .forEach((composite) => {
        composite.interfaces.forEach((compositeInterface) => {
          compositeInterface.subtypes.push(composite);
        });
      });

    // Derive classes
    this.classes
      .forEach((objectType) => {
        objectType.classes.push(objectType);
        objectType.interfaces.forEach((v) => {
          v.classes.push(objectType);
        });
      });

    // MethodTypes
    this.methodTypes = data.methodTypes.map((methodTypeData) => {
      const methodType = new MethodType(this);
      methodType.id = methodTypeData.id;
      methodType.objectType = this.metaObjectById.get(methodTypeData.objectTypeId) as ObjectType;
      methodType.name = methodTypeData.name;

      methodType.objectType.exclusiveMethodTypes.push(methodType);

      methodType.objectType.methodTypes.push(methodType);
      if (methodType.objectType.isInterface) {
        this.composites
          .filter(v => v.interfaces.indexOf(methodType.objectType) > -1)
          .forEach(v => v.methodTypes.push(methodType));
      }

      this.metaObjectById.set(methodType.id, methodType);
      return methodType;
    });

    // RelationTypes
    this.relationTypes = data.relationTypes.map((relationTypeData) => {
      const dataRoleType = relationTypeData.roleType;
      const dataAssociationType = relationTypeData.associationType;

      const relationType = new RelationType(this);
      relationType.id = relationTypeData.id;

      const associationType = new AssociationType(relationType);
      relationType.associationType = associationType;
      associationType.id = dataAssociationType.id;
      associationType.objectType = this.metaObjectById.get(dataAssociationType.objectTypeId) as ObjectType;
      associationType.name = dataAssociationType.name;
      associationType.isOne = dataAssociationType.isOne;

      const roleTypeVirtual = new RoleTypeVirtual();
      roleTypeVirtual.isRequired = dataRoleType.isRequired;

      const roleType = new RoleType(relationType, roleTypeVirtual);
      relationType.roleType = roleType;
      roleType.id = dataRoleType.id;
      roleType.objectType = this.metaObjectById.get(dataRoleType.objectTypeId) as ObjectType;
      roleType.singular = dataRoleType.singular;
      roleType.plural = dataRoleType.plural;
      roleType.isOne = dataRoleType.isOne;
      roleType.name = roleType.isOne ? roleType.singular : roleType.plural;


      if (relationTypeData.concreteRoleTypes) {
        relationTypeData.concreteRoleTypes.forEach((dataConcreteRoleType) => {
          const roleTypeOverride = new RoleTypeVirtual();
          roleTypeOverride.isRequired = dataConcreteRoleType.isRequired;
          const objectType = this.metaObjectById.get(dataConcreteRoleType.objectTypeId) as ObjectType;
          roleType.overridesByClass.set(objectType, roleTypeOverride);
        });
      }

      associationType.objectType.exclusiveRoleTypes.push(roleType);
      roleType.objectType.exclusiveAssociationTypes.push(associationType);

      associationType.objectType.roleTypes.push(roleType);
      roleType.objectType.associationTypes.push(associationType);

      if (associationType.objectType.isInterface) {
        associationType.objectType.subtypes.forEach(subtype => subtype.roleTypes.push(roleType));
      }

      if (roleType.objectType.isInterface) {
        roleType.objectType.subtypes.forEach(subtype => subtype.associationTypes.push(associationType));
      }

      this.metaObjectById.set(relationType.id, relationType);
      this.metaObjectById.set(relationType.roleType.id, relationType.roleType);
      this.metaObjectById.set(relationType.associationType.id, relationType.associationType);

      return relationType;
    });

    // Derive RoleType and AssociationType By Name
    this.composites
      .forEach((objectType) => {
        objectType.roleTypes.forEach(v => objectType.roleTypeByName.set(v.name, v));
        objectType.associationTypes.forEach(v => objectType.associationTypeByName.set(v.name, v));
        objectType.methodTypes.forEach(v => objectType.methodTypeByName.set(v.name, v));

        objectType.subtypes.forEach(subtype => {
          subtype.roleTypes
            .filter(subtypeRoleType => !objectType.roleTypes.find(v => v.id === subtypeRoleType.id))
            .forEach(v => objectType.roleTypeByName.set(`${subtype.name}_${v.name}`, v));

          subtype.associationTypes
            .filter(subtypeAssociationType => !objectType.exclusiveAssociationTypes.find(v => v === subtypeAssociationType))
            .forEach(v => objectType.associationTypeByName.set(`${subtype.name}_${v.name}`, v));
        });

      });

    // Assign Own Properties
    this.composites
      .forEach((objectType) => {
        this[objectType.name] = objectType;
        objectType.roleTypes.forEach((roleType) => objectType[roleType.name] = roleType);
        objectType.associationTypes.forEach((associationType) => objectType[associationType.name] = associationType);
        objectType.methodTypes.forEach((methodTypes) => objectType[methodTypes.name] = methodTypes);
      });

    // Assign Properties from Interfaces
    this.composites
      .forEach((objectType) => {
        objectType.interfaces.forEach((v) => {
          v.roleTypes.forEach((roleType) => objectType[roleType.name] = roleType);
          v.associationTypes.forEach((associationType) => objectType[associationType.name] = associationType);
          v.methodTypes.forEach((methodTypes) => objectType[methodTypes.name] = methodTypes);
        });
      });
  }
}
