import { humanize} from './humanize';

describe('humanize', function () {
  describe('camelCased', function () {
    it('returns humanized', function () {

      const camelCased = "camelCase";
      const humanized = humanize(camelCased);

      expect(humanized).toBe("Camel Case");
    });
  });
});
