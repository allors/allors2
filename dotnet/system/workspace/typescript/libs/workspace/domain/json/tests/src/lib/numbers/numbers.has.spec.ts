import { Numbers, has } from '@allors/workspace/domain/json/system';

describe('Numbers', () => {
  describe('as undefined set', () => {
    const set: Numbers = undefined;

    describe('has a number', () => {
      const hasZero = has(set, 0);

      it('should return undefined', () => {
        expect(hasZero).toBeFalsy();
      });
    });
  });

  describe('as single element set', () => {
    const set: Numbers = Numbers([1]);

    describe('has the element', () => {
      const hasTheElement = has(set, 1);

      it('should be true', () => {
        expect(hasTheElement).toBeTruthy();
      });
    });

    describe('has a non existing element', () => {
      const hasNonExistingElement = has(set, 0);

      it('should be false', () => {
        expect(hasNonExistingElement).toBeFalsy();
      });
    });
  });
});

describe('as multiple element set', () => {
  const set: Numbers = Numbers([3, 1, 6, 5]);

  describe('has the elements 1, 3, 5 and 6', () => {
    const has1 = has(set, 1);
    const has3 = has(set, 3);
    const has5 = has(set, 5);
    const has6 = has(set, 6);

    it('should be true', () => {
      expect(has1).toBeTruthy();
      expect(has3).toBeTruthy();
      expect(has5).toBeTruthy();
      expect(has6).toBeTruthy();
    });
  });

  describe('has non existing elements -1, 0, 2, 4, 7,8', () => {
    const hasMin1 = has(set, -1);
    const has0 = has(set, 0);
    const has2 = has(set, 2);
    const has4 = has(set, 4);
    const has7 = has(set, 7);
    const has8 = has(set, 8);

    it('should be false', () => {
      expect(hasMin1).toBeFalsy();
      expect(has0).toBeFalsy();
      expect(has2).toBeFalsy();
      expect(has4).toBeFalsy();
      expect(has7).toBeFalsy();
      expect(has8).toBeFalsy();
    });
  });
});
