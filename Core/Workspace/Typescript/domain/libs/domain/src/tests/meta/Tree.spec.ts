import { MetaPopulation } from '@allors/workspace/meta';
import { Workspace } from '@allors/workspace/domain';

import { data, TreeFactory, Meta } from '@allors/meta';
import { domain } from '@allors/domain';

describe('Tree',
  () => {
    let m: Meta;
    let factory: TreeFactory;

    beforeEach(async () => {
      m = new MetaPopulation(data) as Meta;
      const workspace = new Workspace(m);
      domain.apply(workspace);

      factory = new TreeFactory(m);
    });

    describe('with empty include',
      () => {
        it('should serialize to correct json', () => {

          const orignal = factory.Organisation({});

          const json = JSON.stringify(orignal);
          const include = JSON.parse(json);

          assert.isArray(include);
          assert.isEmpty(include);
        });
      });

    describe('with one role include',
      () => {
        it('should serialize to correct json', () => {

          const original = factory.Organisation({
            Employees: {},
          });

          const json = JSON.stringify(original);
          const include = JSON.parse(json);

          assert.deepEqual(include, [
            {
              propertytype: 'b95c7b34-a295-4600-82c8-826cc2186a00',
            },
          ]);
        });
      });

    describe('with two roles include',
      () => {
        it('should serialize to correct json', () => {

          const original = factory.Organisation({
            Employees: {},
            Manager: {},
          });

          const json = JSON.stringify(original);
          const include = JSON.parse(json);

          assert.deepEqual(include, [
            {
              propertytype: 'b95c7b34-a295-4600-82c8-826cc2186a00',
            },
            {
              propertytype: '19de0627-fb1c-4f55-9b65-31d8008d0a48',
            },
          ]);
        });
      });

    describe('with a nested role include',
      () => {
        it('should serialize to correct json', () => {

          const original = factory.Organisation({
            Employees: {
              Photo: {},
            },
          });

          const json = JSON.stringify(original);
          const include = JSON.parse(json);

          assert.deepEqual(include, [
            {
              nodes: [{
                propertytype: 'f6624fac-db8e-4fb2-9e86-18021b59d31d',
              }],
              propertytype: 'b95c7b34-a295-4600-82c8-826cc2186a00',
            },
          ]);
        });
      });

    describe('with a subclass role include',
      () => {
        it('should serialize to correct json', () => {

          const original = factory.Deletable({
            Person_Photo: {},
          });

          const json = JSON.stringify(original);
          const include = JSON.parse(json);

          assert.deepEqual(include, [
            {
              propertytype: 'f6624fac-db8e-4fb2-9e86-18021b59d31d',
            },
          ]);
        });
      });

    describe('with a non exsiting role include',
      () => {
        it('should throw exception', () => {

          assert.throw(() => {
            const original = factory.Organisation({
              Oops: {},
            } as any);
          }, Error);
        });
      });
  });
