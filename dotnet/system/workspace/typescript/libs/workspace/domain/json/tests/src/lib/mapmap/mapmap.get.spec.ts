import { MapMap, Numbers } from '@allors/workspace/domain/json/system';
import { mm } from './mm';

describe('MapMap', () => {
  describe('after construction', () => {
    const mapMap = new MapMap<string, string, Numbers>();

    describe('getting a value', () => {
      const value = mapMap.get('a', 'b');

      it('should return undefined', () => {
        expect(value).toBeUndefined();
      });
    });
  });

  describe('with key1 present', () => {
    const mapMap = new MapMap<string, string, Numbers>();
    const mm = ((mapMap as unknown) as { mapMap: Map<string, Map<string, Numbers>> }).mapMap;
    mm.set('a', new Map());

    describe('getting a value', () => {
      const value = mapMap.get('a', 'b');

      it('should return undefined', () => {
        expect(value).toBeUndefined();
      });
    });
  });

  describe('with key1 and key2 present', () => {
    const mapMap = new MapMap<string, string, Numbers>();
    mm(mapMap).set('a', new Map());
    mm(mapMap)
      .get('a')
      ?.set('b', Numbers([0]));

    describe('getting a value', () => {
      const value = mapMap.get('a', 'b');

      it('should return value', () => {
        expect(value).toEqual(Numbers([0]));
      });
    });
  });
});
