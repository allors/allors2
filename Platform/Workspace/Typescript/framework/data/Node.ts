import { ObjectType, PropertyType } from "../meta";

export class Node {
  public propertyType: PropertyType;
  public nodes: Node[];

  constructor(json: any, objectType: ObjectType, propertyTypeName: string) {
    let propertyType =
      objectType.roleTypeByName.get(propertyTypeName) ||
      objectType.associationTypeByName.get(propertyTypeName);

    if (!propertyType) {
      const metaPopulation = objectType.metaPopulation;
      const [subTypeName, subStepName] = propertyTypeName.split("_");

      const subType = metaPopulation.objectTypeByName.get(subTypeName);
      if (subType) {
        propertyType =
          subType.roleTypeByName.get(subStepName) ||
          subType.associationTypeByName.get(propertyTypeName);
      }
    }

    if (!propertyType) {
      throw new Error(
        `No property ${propertyTypeName} found on ${objectType.name}`
      );
    }

    this.propertyType = propertyType;

    const property = json[propertyTypeName];
    this.nodes = property.nodes
      ? property.nodes
      : Object.keys(property).map(
          (childPropertyName) =>
            new Node(property, this.propertyType.objectType, childPropertyName)
        );
  }

  public toJSON(): any {
    return {
      nodes: this.nodes,
      propertytype: this.propertyType.id,
    };
  }
}
