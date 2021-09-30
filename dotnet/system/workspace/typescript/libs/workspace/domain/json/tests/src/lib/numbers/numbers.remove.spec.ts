import { Numbers, remove } from '@allors/workspace/domain/json/system';

describe('Numbers', () => {
  describe('as undefined set', () => {
    const set: Numbers = undefined;

    describe('removing a non existing element', () => {
      const removed = remove(set, 0);

      it('should return undefined', () => {
        expect(removed).toBeUndefined();
      });
    });
  });

  describe('as single element set', () => {
    const set: Numbers = Numbers([1]);

    describe('removing the element', () => {
      const removed = remove(set, 1);

      it('should return undefined', () => {
        expect(removed).toBeUndefined();
      });
    });

    describe('removing a non existing element', () => {
      const removed = remove(set, 0);

      it('should return an array', () => {
        expect(Array.isArray(removed)).toBeTruthy();
      });

      it('should contain one element', () => {
        expect((removed as []).length).toEqual(1);
      });

      it('should only contain the original element', () => {
        expect(removed).toContain(1);
      });
    });
  });
});
