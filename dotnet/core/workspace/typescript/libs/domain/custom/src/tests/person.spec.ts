import { MetaPopulation } from '@allors/meta/system';
import { Session, Workspace } from '@allors/domain/system';
import { Person } from '@allors/domain/generated';

import { data, Meta } from '@allors/meta/generated';

import { extend } from '..';

describe('Person', () => {
  let session: Session;
  let m: Meta;

  beforeEach(() => {
    const metaPopulation = new MetaPopulation(data);
    const workspace = new Workspace(metaPopulation);
    extend(workspace);

    m = metaPopulation as Meta;
    session = new Session(workspace);
  });

  describe('UserName', () => {
    let person: Person;

    beforeEach(() => {
      person = session.create(m.Person) as Person;
    });

    it('should have a UserName', () => {
      const userName = person.UserName;
    });
  });

  describe('displayName', () => {
    let person: Person;

    beforeEach(() => {
      person = session.create(m.Person) as Person;
    });

    it('should be N/A when nothing set', () => {
      expect(person.displayName).toBe('N/A');
    });

    it('should be john@doe.com when username is john@doe.com', () => {
      person.UserName = 'john@doe.com';
      expect(person.displayName).toBe('john@doe.com');
    });

    it('should be Doe when lastName is Doe', () => {
      person.LastName = 'Doe';
      expect(person.displayName).toBe('Doe');
    });

    it('should be John with firstName John', () => {
      person.FirstName = 'John';
      expect(person.displayName).toBe('John');
    });

    it('should be John Doe (even twice) with firstName John and lastName Doe', () => {
      person.FirstName = 'John';
      person.LastName = 'Doe';

      expect(person.displayName).toBe('John Doe');
      expect(person.displayName).toBe('John Doe');
    });
  });

  describe('hello', () => {
    let person: Person;

    beforeEach(() => {
      person = session.create(m.Person) as Person;
    });

    it('should be Hello John Doe when lastName Doe and firstName John', () => {
      person.LastName = 'Doe';
      person.FirstName = 'John';

      expect(person.hello()).toBe('Hello John Doe');
    });
  });
});
