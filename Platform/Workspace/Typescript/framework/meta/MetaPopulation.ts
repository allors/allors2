import { MetaData, ObjectTypeData } from "./Data";

import { MetaObject } from "./MetaObject";
import { Kind, ObjectType } from "./ObjectType";
import { unitIdByTypeName } from "./Units";
import { RelationType } from "./RelationType";
import { MethodType } from "./MethodType";
import { RoleType, RoleTypeVirtual } from "./RoleType";
import { AssociationType } from "./AssociationType";

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
    this.units = [
      "Binary",
      "Boolean",
      "DateTime",
      "Decimal",
      "Float",
      "Integer",
      "String",
      "Unique",
    ].map((unitName) => {
      const objectType = new ObjectType(
        this,
        unitIdByTypeName[unitName],
        unitName,
        unitName === "Binary" ? "Binaries" : unitName + "s",
        Kind.unit
      );

      this.objectTypes.push(objectType);
      this.objectTypeByName.set(objectType.name, objectType);
      this.metaObjectById.set(objectType.id, objectType);
      return objectType;
    });

    const dataInterfaces = data.interfaces ?? [];
    const dataClasses = data.classes ?? [];
    const dataMethodTypes = data.methodTypes ?? [];
    const dataRelationTypes = data.relationTypes ?? [];

    // Interfaces
    this.interfaces =
      dataInterfaces.map((interfaceData) => {
        const objectType = new ObjectType(
          this,
          interfaceData.id,
          interfaceData.name,
          interfaceData.plural,
          Kind.interface
        );

        this.composites.push(objectType);
        this.objectTypes.push(objectType);
        this.objectTypeByName.set(objectType.name, objectType);
        this.metaObjectById.set(objectType.id, objectType);
        return objectType;
      }) ?? [];

    // Classes
    this.classes =
      dataClasses.map((classData) => {
        const objectType = new ObjectType(
          this,
          classData.id,
          classData.name,
          classData.plural,
          Kind.class
        );

        this.composites.push(objectType);
        this.objectTypes.push(objectType);
        this.objectTypeByName.set(objectType.name, objectType);
        this.metaObjectById.set(objectType.id, objectType);
        return objectType;
      }) ?? [];

    const dataObjectTypes = ([] as ObjectTypeData[])
      .concat(dataInterfaces)
      .concat(dataClasses);

    // Create Type Hierarchy
    dataObjectTypes.forEach((dataObjectType) => {
      const objectType = this.metaObjectById.get(
        dataObjectType.id
      ) as ObjectType;
      objectType.interfaces = dataObjectType.interfaceIds
        ? dataObjectType.interfaceIds.map(
            (v) => this.metaObjectById.get(v) as ObjectType
          )
        : [];
    });

    // Derive subtypes
    this.composites.forEach((composite) => {
      composite.interfaces.forEach((compositeInterface) => {
        compositeInterface.subtypes.push(composite);
      });
    });

    // Derive classes
    this.classes.forEach((objectType) => {
      objectType.classes.push(objectType);
      objectType.interfaces.forEach((v) => {
        v.classes.push(objectType);
      });
    });

    // MethodTypes
    this.methodTypes = dataMethodTypes.map((methodTypeData) => {
      const methodType = new MethodType(
        this,
        methodTypeData.id,
        this.metaObjectById.get(methodTypeData.objectTypeId) as ObjectType,
        methodTypeData.name
      );

      methodType.objectType.exclusiveMethodTypes.push(methodType);

      methodType.objectType.methodTypes.push(methodType);
      if (methodType.objectType.isInterface) {
        this.composites
          .filter((v) => v.interfaces.indexOf(methodType.objectType) > -1)
          .forEach((v) => v.methodTypes.push(methodType));
      }

      this.metaObjectById.set(methodType.id, methodType);
      return methodType;
    });

    // RelationTypes
    this.relationTypes = dataRelationTypes.map((relationTypeData) => {
      const relationType = new RelationType(this, relationTypeData);

      this.metaObjectById.set(relationType.id, relationType);
      this.metaObjectById.set(relationType.roleType.id, relationType.roleType);
      this.metaObjectById.set(
        relationType.associationType.id,
        relationType.associationType
      );

      return relationType;
    });

    // Derive RoleType and AssociationType By Name
    this.composites.forEach((objectType) => {
      objectType.roleTypes.forEach((v) =>
        objectType.roleTypeByName.set(v.name, v)
      );
      objectType.associationTypes.forEach((v) =>
        objectType.associationTypeByName.set(v.name, v)
      );
      objectType.methodTypes.forEach((v) =>
        objectType.methodTypeByName.set(v.name, v)
      );

      objectType.subtypes.forEach((subtype) => {
        subtype.roleTypes
          .filter(
            (subtypeRoleType) =>
              !objectType.roleTypes.find((v) => v.id === subtypeRoleType.id)
          )
          .forEach((v) =>
            objectType.roleTypeByName.set(`${subtype.name}_${v.name}`, v)
          );

        subtype.associationTypes
          .filter(
            (subtypeAssociationType) =>
              !objectType.exclusiveAssociationTypes.find(
                (v) => v === subtypeAssociationType
              )
          )
          .forEach((v) =>
            objectType.associationTypeByName.set(`${subtype.name}_${v.name}`, v)
          );
      });
    });

    // Assign Own Properties
    this.composites.forEach((objectType) => {
      (this as any)[objectType.name] = objectType;
      objectType.roleTypes.forEach(
        (roleType) => ((objectType as any)[roleType.name] = roleType)
      );
      objectType.associationTypes.forEach(
        (associationType) =>
          ((objectType as any)[associationType.name] = associationType)
      );
      objectType.methodTypes.forEach(
        (methodTypes) => ((objectType as any)[methodTypes.name] = methodTypes)
      );
    });

    // Assign Properties from Interfaces
    this.composites.forEach((objectType) => {
      objectType.interfaces.forEach((v) => {
        v.roleTypes.forEach(
          (roleType) => ((objectType as any)[roleType.name] = roleType)
        );
        v.associationTypes.forEach(
          (associationType) =>
            ((objectType as any)[associationType.name] = associationType)
        );
        v.methodTypes.forEach(
          (methodTypes) => ((objectType as any)[methodTypes.name] = methodTypes)
        );
      });
    });
  }
}
