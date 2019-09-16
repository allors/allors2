import { domain, Organisation, Person } from '../../src/allors/domain';
import { MetaPopulation, PullRequest, Session, Workspace } from '../../src/allors/framework';
import { data, TreeFactory } from '../../src/allors/meta';

import { assert } from 'chai';
import 'mocha';

describe('Tree',
  () => {
    let metaPopulation: MetaPopulation;
    let factory: TreeFactory;

    beforeEach(async () => {
      metaPopulation = new MetaPopulation(data);
      const workspace = new Workspace(metaPopulation);
      domain.apply(workspace);

      factory = new TreeFactory(metaPopulation);
    });

    describe('with empty include',
      () => {
        it('should serialize to correct json', () => {

          const orignal = factory.Organisation({});

          const json = JSON.stringify(orignal);
          const include = JSON.parse(json).nodes;

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
          const include = JSON.parse(json).nodes;

          assert.deepEqual(include, [
            {
              propertytype: 'b95c7b34a295460082c8826cc2186a00',
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
          const include = JSON.parse(json).nodes;

          assert.deepEqual(include, [
            {
              propertytype: 'b95c7b34a295460082c8826cc2186a00',
            },
            {
              propertytype: '19de0627fb1c4f559b6531d8008d0a48',
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
          const include = JSON.parse(json).nodes;

          assert.deepEqual(include, [
            {
              nodes: [{
                propertytype: 'f6624facdb8e4fb29e8618021b59d31d',
              }],
              propertytype: 'b95c7b34a295460082c8826cc2186a00',
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
          const include = JSON.parse(json).nodes;

          assert.deepEqual(include, [
            {
              propertytype: 'f6624facdb8e4fb29e8618021b59d31d',
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
