import { ObjectType, PropertyType } from '../meta';

export class Node {

  public propertyType: PropertyType;
  public nodes: Node[];

  constructor(fields?: Partial<Node>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      nodes: this.nodes,
      propertytype: this.propertyType.id,
    };
  }

  public parse(json: any, objectType: ObjectType, propertyTypeName: string) {
    let propertyType = objectType.roleTypeByName.get(propertyTypeName) || objectType.associationTypeByName.get(propertyTypeName);

    if (!propertyType) {
      const metaPopulation = objectType.metaPopulation;
      const [subTypeName, subStepName] = propertyTypeName.split('_');

      const subType = metaPopulation.objectTypeByName.get(subTypeName);
      if (subType) {
        propertyType = subType.roleTypeByName.get(subStepName) || subType.associationTypeByName.get(propertyTypeName);
      }
    }

    if (!propertyType) {
      throw new Error(`No property ${propertyTypeName} found on ${objectType.name}`)
    }

    this.propertyType = propertyType;

    const property = json[propertyTypeName];
    if (property.nodes) {
      this.nodes = property.nodes;
    } else if (property) {
      const nodes = Object.keys(property)
        .map((childPropertyName) => {
          const childTreeNode = new Node();
          childTreeNode.parse(property, this.propertyType.objectType, childPropertyName);
          return childTreeNode;
        });

      if (nodes && nodes.length) {
        this.nodes = nodes;
      }
    }
  }
}
