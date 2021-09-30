import { Numbers } from '@allors/workspace/domain/json/system';

describe('Numbers', () => {
  describe('initialize with undefined', () => {
    const set = Numbers(undefined);

    it('should return undefined', () => {
      expect(set).toBeUndefined();
    });
  });
});
