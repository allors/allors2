import { CreateSchemaCustomizationArgs, NodePluginSchema } from 'gatsby';

import { SchemaBuilder } from '@allors/gatsby/source/core';

import { workspace } from '../src';

import 'jest-extended';

export class FakeSchema implements NodePluginSchema {
  buildObjectType(config: any): any {
    return this.allTypes.push(config);
  }

  buildUnionType(config: any): any {
    return this.allTypes.push(config);
  }

  buildInterfaceType(config: any): any {
    return this.allTypes.push(config);
  }

  buildInputObjectType(config: any): any {
    return this.allTypes.push(config);
  }

  buildEnumType(config: any): any {
    return this.allTypes.push(config);
  }

  buildScalarType(config: any): any {
    return this.allTypes.push(config);
  }

  allTypes: (string | object | Array<string | object>)[] = [];

  createTypes = (types: string | object | Array<string | object>, plugin?: any, traceId?: string): void => {
    this.allTypes.push(types);
  };
}

describe('SchemaBuilder', () => {
  describe('build', () => {
    it('should create the types', async () => {
      const fakeSchema = new FakeSchema();

      const args = {
        actions: {
          createTypes: fakeSchema.createTypes,
        },
        schema: fakeSchema as NodePluginSchema,
      } as CreateSchemaCustomizationArgs;

      const gatsby = new SchemaBuilder(workspace.metaPopulation, args, { plugins: [] });
      await gatsby.build();

      expect(fakeSchema.allTypes).not.toBeEmpty();
    });
  });
});
