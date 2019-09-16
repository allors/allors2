import { ObjectType, PropertyType } from '../meta';

export class TreeNode {

  public propertyType: PropertyType;
  public nodes: TreeNode[];

  constructor(fields?: Partial<TreeNode>) {
    Object.assign(this, fields);
  }

  public toJSON(): any {
    return {
      nodes: this.nodes,
      propertytype: this.propertyType.id,
    };
  }

  public parse(json: any, objectType: ObjectType, propertyTypeName: string) {
    this.propertyType = objectType.roleTypeByName[propertyTypeName] || objectType.associationTypeByName[propertyTypeName];

    if (!this.propertyType) {
      const metaPopulation = objectType.metaPopulation;
      const [subTypeName, subStepName] = propertyTypeName.split('_');

      const subType = metaPopulation.objectTypeByName[subTypeName];
      if (subType) {
        this.propertyType = subType.roleTypeByName[subStepName] || subType.associationTypeByName[propertyTypeName];
      }
    }

    if (!this.propertyType) {
      throw new Error('Unknown Property: ' + propertyTypeName);
    }

    const property = json[propertyTypeName];
    if (property.nodes) {
      this.nodes = property.nodes;
    } else if (property) {
      const nodes = Object.keys(property)
        .map((childPropertyName) => {
          const childTreeNode = new TreeNode();
          childTreeNode.parse(property, this.propertyType.objectType, childPropertyName);
          return childTreeNode;
        });

      if (nodes && nodes.length) {
        this.nodes = nodes;
      }
    }
  }
}
