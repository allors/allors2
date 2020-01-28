
import { assert } from 'chai';
import 'mocha';
import { SchemaBuilder } from '../gatsby/SchemaBuilder';
import { CreateSchemaCustomizationArgs } from 'gatsby';
import metaPopulation from '../metaPopulation';

class FakeSchema {
  public allTypes = [];

  createTypes = (types: string | object | Array<string | object>, plugin?: any, traceId?: string): void  => {
    this.allTypes.push(types);
  }
}

describe('SchemaBuilder',
  () => {
    describe('build',
      () => {
        it('should create the types', async () => {

          const fakeSchema = new FakeSchema();

          const args = {
            actions: {
              createTypes: fakeSchema.createTypes,
            },
          } as CreateSchemaCustomizationArgs

          var gatsby = new SchemaBuilder(metaPopulation, args, { plugins: [] });
          await gatsby.build();

          assert.isNotEmpty(fakeSchema.allTypes);
        });
      });
  });
