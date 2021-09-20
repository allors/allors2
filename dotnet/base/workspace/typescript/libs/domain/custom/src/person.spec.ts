import { Meta, data } from '@allors/meta/generated';
import { Session, Workspace } from '@allors/domain/system';
import { MetaPopulation } from '@allors/meta/system';
import { Person } from '@allors/domain/generated';

import { extend } from '../src';

describe('Person', () => {
  let m: Meta;
  let session: Session;

  beforeEach(() => {
    m = new MetaPopulation(data) as Meta;
    const workspace = new Workspace(m);
    extend(workspace);

    session = new Session(workspace);
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
});
