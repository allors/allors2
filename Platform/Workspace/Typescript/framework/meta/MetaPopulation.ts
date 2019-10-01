import { Class, Data, Interface } from './Data';

import { MetaObject } from './MetaObject';
import { Kind, ObjectType } from './ObjectType';
import { unitIdByTypeName } from './Units';
import { RelationType } from './RelationType';
import { ConcreteRoleType } from './ConcreteRoleType';
import { MethodType } from './MethodType';

export class MetaPopulation {
  readonly metaObjectById: { [id: string]: MetaObject; } = {};

  readonly xobjectTypeByName: { [name: string]: ObjectType } = {};

  readonly units: ObjectType[] = [];
  readonly composites: ObjectType[] = [];
  readonly interfaces: ObjectType[] = [];
  readonly classes: ObjectType[] = [];
  readonly objectTypes: ObjectType[] = [];
  readonly relationTypes: RelationType[] = [];
  readonly methodTypes: MethodType[] = [];

  constructor(data: Data) {

    // Units
    ['Binary', 'Boolean', 'DateTime', 'Decimal', 'Float', 'Integer', 'String', 'Unique']
      .forEach((name) => {
        const objectType = new ObjectType(this);
        objectType.id = unitIdByTypeName[name];
        objectType.name = name;
        objectType.plural = name === 'Binary' ? 'Binaries' : name + 's';
        objectType.kind = Kind.unit;
        this.units.push(objectType);
        this.objectTypes.push(objectType);
        this.xobjectTypeByName[objectType.name] = objectType;
        this.metaObjectById[objectType.id] = objectType;
      });

    // Interfaces
    data.interfaces.forEach((dataInterface) => {
      const objectType = new ObjectType(this);
      objectType.id = dataInterface.id;
      objectType.name = dataInterface.name;
      objectType.plural = dataInterface.plural;
      objectType.kind = Kind.interface;
      this.composites.push(objectType);
      this.interfaces.push(objectType);
      this.objectTypes.push(objectType);
      this.xobjectTypeByName[objectType.name] = objectType;
      this.metaObjectById[objectType.id] = objectType;
    });

    // Classes
    data.classes.forEach((dataClass) => {
      const objectType = new ObjectType(this);
      objectType.id = dataClass.id;
      objectType.name = dataClass.name;
      objectType.plural = dataClass.plural;
      objectType.kind = Kind.class;
      this.composites.push(objectType);
      this.classes.push(objectType);
      this.objectTypes.push(objectType);
      this.xobjectTypeByName[objectType.name] = objectType;
      this.metaObjectById[objectType.id] = objectType;
    });

    const dataObjectTypes = [].concat(data.interfaces).concat(data.classes);

    // Implemented interfaces
    dataObjectTypes.forEach((dataObjectType: Interface | Class) => {
      const metaObjectType = this.metaObjectById[dataObjectType.id] as ObjectType;
      metaObjectType.interfaces = dataObjectType.interfaceIds ? dataObjectType.interfaceIds.map((v) => this.metaObjectById[v] as ObjectType) : [];
    });

    // RelationTypes
    data.relationTypes.forEach((dataRelationType) => {
      const relationType = new RelationType(this);
      relationType.id = dataRelationType.id;

      const dataAssociationType = dataRelationType.associationType;
      const associationType = relationType.associationType;
      associationType.id = dataAssociationType.id;
      associationType.objectType = this.metaObjectById[dataAssociationType.objectTypeId] as ObjectType;
      associationType.name = dataAssociationType.name;
      associationType.isOne = dataAssociationType.isOne;

      const dataRoleType = dataRelationType.roleType;
      const roleType = relationType.roleType;
      roleType.id = dataRoleType.id;
      roleType.objectType = this.metaObjectById[dataRoleType.objectTypeId] as ObjectType;
      roleType.singular = dataRoleType.singular;
      roleType.plural = dataRoleType.plural;
      roleType.isOne = dataRoleType.isOne;
      roleType.isRequired = dataRoleType.isRequired;

      roleType.name = roleType.isOne ? roleType.singular : roleType.plural;

      if (dataRelationType.concreteRoleTypes) {
        dataRelationType.concreteRoleTypes.forEach((dataConcreteRoleType) => {
          const concreteRoleType = new ConcreteRoleType(this);
          concreteRoleType.relationType = relationType;
          concreteRoleType.roleType = roleType;
          concreteRoleType.isRequired = dataConcreteRoleType.isRequired;

          const objectType = this.metaObjectById[dataConcreteRoleType.objectTypeId] as ObjectType;
          relationType.concreteRoleTypeByClassId[objectType.id] = concreteRoleType;
        });
      }

      associationType.objectType.xroleTypeByName[roleType.name] = roleType;
      roleType.objectType.xassociationTypeByName[associationType.name] = associationType;

      this.relationTypes.push(relationType);
      this.metaObjectById[relationType.id] = relationType;
      this.metaObjectById[relationType.roleType.id] = relationType.roleType;
      this.metaObjectById[relationType.associationType.id] = relationType.associationType;
    });

    // RelationTypes
    data.methodTypes.forEach((dataMethodType) => {
      const methodType = new MethodType(this);
      methodType.id = dataMethodType.id;
      methodType.objectType = this.metaObjectById[dataMethodType.objectTypeId] as ObjectType;
      methodType.name = dataMethodType.name;

      methodType.objectType.xmethodTypeByName[methodType.name] = methodType;

      this.methodTypes.push(methodType);
      this.metaObjectById[methodType.id] = methodType;
    });

    // Derive
    this.objectTypes
      .forEach((objectType) => objectType.derive());

    // Statically typed access
    this.composites
      .forEach((objectType) => {
        this[objectType.name] = objectType;

        Object.keys(objectType.xroleTypeByName).forEach((name) => {
          const roleType = objectType.xroleTypeByName[name];
          objectType[name] = roleType;
        });

        Object.keys(objectType.xassociationTypeByName).forEach((name) => {
          objectType[name] = objectType.xassociationTypeByName[name];
        });

        Object.keys(objectType.xmethodTypeByName).forEach((name) => {
          objectType[name] = objectType.xmethodTypeByName[name];
        });
      });
  }
}
