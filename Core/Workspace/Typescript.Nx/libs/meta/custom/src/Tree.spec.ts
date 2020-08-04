import { MetaPopulation } from '@allors/meta/system';
import { Workspace } from '@allors/domain/system';

import { data, TreeFactory, Meta } from '@allors/meta/generated';

import 'jest-extended';

describe('Tree', () => {
  let m: Meta;
  let factory: TreeFactory;
  let workspace: Workspace;

  beforeEach(async () => {
    m = new MetaPopulation(data) as Meta;
    workspace = new Workspace(m);

    factory = new TreeFactory(m);
  });

  describe('with empty include', () => {
    it('should serialize to correct json', () => {
      const orignal = factory.Organisation({});

      const json = JSON.stringify(orignal);
      const include = JSON.parse(json);

      expect(include).toBeArray();
      expect(include).toBeEmpty();
    });
  });

  describe('with one role include', () => {
    it('should serialize to correct json', () => {
      const original = factory.Organisation({
        Employees: {},
      });

      const json = JSON.stringify(original);
      const include = JSON.parse(json);

      expect(include).toEqual([
        {
          propertytype: 'b95c7b34-a295-4600-82c8-826cc2186a00',
        },
      ]);
    });
  });

  describe('with two roles include', () => {
    it('should serialize to correct json', () => {
      const original = factory.Organisation({
        Employees: {},
        Manager: {},
      });

      const json = JSON.stringify(original);
      const include = JSON.parse(json);

      expect(include).toEqual([
        {
          propertytype: 'b95c7b34-a295-4600-82c8-826cc2186a00',
        },
        {
          propertytype: '19de0627-fb1c-4f55-9b65-31d8008d0a48',
        },
      ]);
    });
  });

  describe('with a nested role include', () => {
    it('should serialize to correct json', () => {
      const original = factory.Organisation({
        Employees: {
          Photo: {},
        },
      });

      const json = JSON.stringify(original);
      const include = JSON.parse(json);

      expect(include).toEqual([
        {
          nodes: [
            {
              propertytype: 'f6624fac-db8e-4fb2-9e86-18021b59d31d',
            },
          ],
          propertytype: 'b95c7b34-a295-4600-82c8-826cc2186a00',
        },
      ]);
    });
  });

  describe('with a subclass role include', () => {
    it('should serialize to correct json', () => {
      const original = factory.Deletable({
        Person_Photo: {},
      });

      const json = JSON.stringify(original);
      const include = JSON.parse(json);

      expect(include).toEqual([
        {
          propertytype: 'f6624fac-db8e-4fb2-9e86-18021b59d31d',
        },
      ]);
    });
  });

  describe('with a non exsiting role include', () => {
    it('should throw exception', () => {
      expect(() => {
        const original = factory.Organisation({
          Oops: {},
        } as any);
      }).toThrowError(Error);
    });
  });
});
