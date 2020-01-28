import { CreateSchemaCustomizationArgs, PluginOptions } from "gatsby"
import { MetaPopulation, ObjectType } from "../framework";
import { Meta } from "../meta";
import createName from "./utils/createName";

export class SchemaBuilder {

  m: Meta;

  constructor(private metaPopulation: MetaPopulation, private args: CreateSchemaCustomizationArgs, { plugins, ...options }: PluginOptions) {

    this.m = this.metaPopulation as Meta;
  }

  public build() {

    const { actions: { createTypes } } = this.args;

    createTypes(`interface AllorsObject {
      allorsId: String!,
      allorsVersion: String!,
      allorsType: String!,
    }`)

    this.metaPopulation.interfaces.forEach((v) => {
      if (v._isGatsby && v._isUnion) {
        const union = v.classes.map((w) => `${w._name}`).join("| ");
        const typeDefs = `union ${v._name} = ${union}\n`
        createTypes(typeDefs)
      }
    });

    this.metaPopulation.classes.forEach((v) => {
      if (v._isGatsby) {
        if (v.gatsbyRoleTypes.length > 0 || v.gatsbyAssociationTypes.length > 0) {
          const fields = this.fields(v);
          const typeDefs = `
          type Allors${v.name} implements Node & AllorsObject {
            ${fields}
            allorsId: String!
            allorsVersion: String!
            allorsType: String!
          }
        `
          createTypes(typeDefs)
        }
      }
    });
  }

  private fields(objectType: ObjectType) {
    let properties = "";
    objectType.gatsbyRoleTypes.forEach((roleType) => {
      let typeName;
      if (roleType.objectType.isUnit) {
        typeName = `${roleType.objectType._name}`;
      } else {
        if (roleType.isOne) {
          typeName = `${roleType.objectType._name} @link`
        } else {
          typeName = `[${roleType.objectType._name}] @link`
        }
      }

      properties += `${createName(roleType.name)}: ${typeName}\n`
    })

    objectType.gatsbyAssociationTypes.forEach((associationType) => {
      let typeName;
      if (associationType.isOne) {
        typeName = `${associationType.objectType._name} @link`
      } else {
        typeName = `[${associationType.objectType._name}] @link`
      }
      properties += `${createName(associationType.name)}: ${typeName}\n`
    })

    // Properties
    if (objectType.gatsbyProperties) {
      objectType.gatsbyProperties.forEach((property => {
        properties += `${createName(property.name)}: ${property.type}\n`
      }))
    }

    return properties;
  }
}
