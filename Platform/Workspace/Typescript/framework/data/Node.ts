import { ObjectType, PropertyType } from '@allors/framework';

export type NodeArgs = Pick<Node, 'propertyType' | 'nodes'>;

export class Node {
  public propertyType: PropertyType;
  public nodes?: Node[];

  constructor(args: NodeArgs);
  constructor(objectType: ObjectType, propertyTypeName: string, json: any);
  constructor(args: ObjectType | NodeArgs, propertyTypeName?: string, json?: any) {

    if(args instanceof ObjectType){
      const objectType = args;
      let propertyType = objectType.roleTypeByName.get(propertyTypeName!) || objectType.associationTypeByName.get(propertyTypeName!);

      if (!propertyType) {
        const metaPopulation = objectType.metaPopulation;
        const [subTypeName, subStepName] = propertyTypeName!.split('_');
  
        const subType = metaPopulation.objectTypeByName.get(subTypeName);
        if (subType) {
          propertyType = subType.roleTypeByName.get(subStepName) || subType.associationTypeByName.get(propertyTypeName!);
        }
      }
  
      if (!propertyType) {
        throw new Error(`No property ${propertyTypeName} found on ${objectType.name}`);
      }
  
      this.propertyType = propertyType;
  
      const property = json[propertyTypeName!];
      this.nodes = property.nodes
        ? property.nodes
        : Object.keys(property).map((childPropertyName) => new Node(this.propertyType.objectType, childPropertyName, property));
    } else{
      Object.assign(this, args);
      this.propertyType = (args as NodeArgs).propertyType;
    }
  }

  public toJSON(): any {
    return {
      nodes: this.nodes && this.nodes.length > 0 ? this.nodes : undefined,
      propertytype: this.propertyType.id,
    };
  }
}
