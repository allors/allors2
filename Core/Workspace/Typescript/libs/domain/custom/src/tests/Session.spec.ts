import { MetaPopulation } from '@allors/meta/system';
import { Session, Workspace } from '@allors/domain/system';
import { PushResponse, ResponseType } from '@allors/protocol/system';

import { data, Meta } from '@allors/meta/generated';
import { Organisation, Person} from '@allors/domain/generated';

import { syncResponse, securityResponse } from './fixture';
import { PushRequestInfo } from './helpers/PushRequestInfo';

import { extend } from '..'

import 'jest-extended';

describe('Session', () => {
  let m: Meta;
  let workspace: Workspace;

  beforeEach(() => {
    m = new MetaPopulation(data) as Meta;
    workspace = new Workspace(m);
    extend(workspace);

    workspace.sync(syncResponse(m));
    workspace.security(securityResponse(m));
  });

  describe('delete', () => {
    let session: Session;

    beforeEach(() => {
      session = new Session(workspace);
    });

    it('should throw exception for existing object', () => {
      const koen = session.get('1') as Person;
      expect(() => {
        session.delete(koen);
      }).toThrow();
    });

    it('should not throw exception for a new object', () => {
      const jos = session.create(m.Person) as Person;
      expect(() => {
        session.delete(jos);
      }).not.toThrow();
    });

    it('should throw exception for a deleted object', () => {
      const acme = session.create(m.Organisation) as Organisation;

      session.delete(acme);

      expect(acme.CanReadName).toBeUndefined();
      expect(acme.CanWriteName).toBeUndefined();
      expect(acme.Name).toBeUndefined();

      expect(() => {
        acme.Name = 'Acme';
      }).toThrow();

      const jos = session.create(m.Person) as Person;

      expect(acme.CanReadOwner).toBeUndefined();
      expect(acme.CanWriteOwner).toBeUndefined();
      expect(acme.Owner).toBeUndefined();

      expect(() => {
        acme.Owner = jos;
      }).toThrow();

      expect(acme.CanReadEmployees).toBeUndefined();
      expect(acme.CanWriteEmployees).toBeUndefined();
      expect(acme.Employees).toBeUndefined();

      expect(() => {
        acme.AddEmployee(jos);
      }).toThrow();

      expect(acme.CanExecuteJustDoIt).toBeUndefined();
      expect(acme.JustDoIt).toBeUndefined();
    });

    it('should delete role from existing object', () => {
      const acme = session.get('101') as Organisation;
      const jos = session.create(m.Person) as Person;

      acme.Owner = jos;

      session.delete(jos);

      expect(acme.Owner).toBeNull();
    });

    it('should remove role from existing object', () => {
      const acme = session.get('101') as Organisation;
      const jos = session.create(m.Person) as Person;

      acme.AddEmployee(jos);

      session.delete(jos);

      expect(acme.Employees).not.toContain(jos);
    });

    it('should delete role from new object', () => {
      const acme = session.create(m.Organisation) as Organisation;
      const jos = session.create(m.Person) as Person;

      acme.Owner = jos;

      session.delete(jos);

      expect(acme.Owner).toBeNull();
    });

    it('should remove role from new object', () => {
      const acme = session.create(m.Organisation) as Organisation;
      const jos = session.create(m.Person) as Person;

      acme.AddEmployee(jos);

      session.delete(jos);

      expect(acme.Employees).not.toContain( jos);
    });
  });

  describe('sync', () => {
    let session: Session;

    beforeEach(() => {
      session = new Session(workspace);
    });

    it('should get unit roles', () => {
      const koen = session.get('1') as Person;

      expect(koen.FirstName).toBe('Koen');
      expect(koen.MiddleName).toBeNull();
      expect(koen.LastName).toBe( 'Van Exem');
      expect(koen.BirthDate).toBe( '1973-03-27T18:00:00Z');
      expect(koen.IsStudent).toBeTruthy();

      const patrick = session.get('2') as Person;

      expect(patrick.FirstName).toBe( 'Patrick');
      expect(patrick.MiddleName).toBeNull();
      expect(patrick.LastName).toBe( 'De Boeck');
      expect(patrick.BirthDate).toBeNull();
      expect(patrick.IsStudent).toBeFalsy();

      const martien = session.get('3') as Person;

      expect(martien.FirstName).toBe( 'Martien');
      expect(martien.MiddleName).toBe( 'van');
      expect(martien.LastName).toBe( 'Knippenberg');
      expect(martien.BirthDate).toBeNull();
      expect(martien.IsStudent).toBeNull();
    });

    it('should get composite roles', () => {
      const koen = session.get('1') as Person;
      const patrick = session.get('2') as Person;
      const martien = session.get('3') as Person;

      const acme = session.get('101') as Organisation;

      expect(acme.Owner).toBe( koen);
      expect(acme.Manager).toBeNull();

      const ocme = session.get('102') as Organisation;

      expect(ocme.Owner).toBe( patrick);
      expect(ocme.Manager).toBeNull();

      const icme = session.get('103') as Organisation;

      expect(icme.Owner).toBe( martien);
      expect(icme.Manager).toBeNull();
    });

    it('should get composites roles', () => {
      const koen = session.get('1') as Person;
      const patrick = session.get('2') as Person;
      const martien = session.get('3') as Person;

      const acme = session.get('101') as Organisation;
      const ocme = session.get('102') as Organisation;
      const icme = session.get('103') as Organisation;

      expect(acme.Employees).toIncludeSameMembers([koen, patrick, martien]);
      expect(ocme.Employees).toIncludeSameMembers( [koen]);
      expect(icme.Employees).toIncludeSameMembers( []);

      expect(acme.Shareholders).toIncludeSameMembers( []);
      expect(ocme.Shareholders).toIncludeSameMembers( []);
      expect(icme.Shareholders).toIncludeSameMembers( []);
    });

    describe('two different sessions with same objects', () => {
      let session1: Session;
      let session2: Session;

      let koen1: Person;
      let patrick1: Person;
      let martien1: Person;

      let acme1: Organisation;
      let ocme1: Organisation;
      let icme1: Organisation;

      let koen2: Person;
      let patrick2: Person;
      let martien2: Person;

      let acme2: Organisation;
      let ocme2: Organisation;
      let icme2: Organisation;

      beforeEach(() => {
        session1 = new Session(workspace);
        session2 = new Session(workspace);

        koen1 = session1.get('1') as Person;
        patrick1 = session1.get('2') as Person;
        martien1 = session1.get('3') as Person;

        acme1 = session1.get('101') as Organisation;
        ocme1 = session1.get('102') as Organisation;
        icme1 = session1.get('103') as Organisation;

        koen2 = session2.get('1') as Person;
        patrick2 = session2.get('2') as Person;
        martien2 = session2.get('3') as Person;

        acme2 = session2.get('101') as Organisation;
        ocme2 = session2.get('102') as Organisation;
        icme2 = session2.get('103') as Organisation;
      });

      describe('update unit roles', () => {
        beforeEach(() => {
          martien2.FirstName = 'Martinus';
          martien2.MiddleName = 'X';
        });

        it('should see changes in this session', () => {
          expect(martien1.FirstName).toBe( 'Martien');
          expect(martien1.LastName).toBe( 'Knippenberg');
          expect(martien1.MiddleName).toBe( 'van');
        });

        it('should not see changes in the other session', () => {
          expect(martien2.FirstName).toBe( 'Martinus');
          expect(martien2.LastName).toBe( 'Knippenberg');
          expect(martien2.MiddleName).toBe( 'X');
        });
      });

      describe('update composite roles', () => {
        beforeEach(() => {
          acme2.Owner = martien2;
          ocme2.Owner = null;
          acme2.Manager = patrick2;
        });

        it('should see changes in this session', () => {
          expect(acme1.Owner).toBe( koen1);
          expect(ocme1.Owner).toBe( patrick1);
          expect(icme1.Owner).toBe( martien1);

          expect(acme1.Manager).toBeNull();
          expect(ocme1.Manager).toBeNull();
          expect(icme1.Manager).toBeNull();
        });

        it('should not see changes in the other session', () => {
          expect(acme2.Owner).toBe( martien2);
          expect(ocme2.Owner).toBeNull();
          expect(icme2.Owner).toBe( martien2);

          expect(acme2.Manager).toBe( patrick2);
          expect(ocme2.Manager).toBeNull();
          expect(icme2.Manager).toBeNull();
        });
      });

      describe('update composites roles', () => {
        beforeEach(() => {
          acme2.Employees = [];
          icme2.Employees = [koen2, patrick2, martien2];
        });

        it('should see changes in this session', () => {
          expect(acme1.Employees).toIncludeSameMembers([koen1, patrick1, martien1]);
          expect(ocme1.Employees).toIncludeSameMembers( [koen1]);
          expect(icme1.Employees).toIncludeSameMembers( []);
        });

        it('should not see changes in the other session', () => {
          expect(acme2.Employees).toIncludeSameMembers( []);
          expect(ocme2.Employees).toIncludeSameMembers( [koen2]);
          expect(icme2.Employees).toIncludeSameMembers( [koen2, patrick2, martien2]);
        });
      });
    });

    it('pushRequest should have all changes from session', () => {
      const koen = session.get('1') as Person;
      const patrick = session.get('2') as Person;
      const martien = session.get('3') as Person;

      const acme = session.get('101') as Organisation;
      const ocme = session.get('102') as Organisation;
      const icme = session.get('103') as Organisation;

      acme.Owner = martien;
      ocme.Owner = null;

      acme.Manager = patrick;

      const save = session.pushRequest();
      const info = new PushRequestInfo(save, m);

      expect(save.objects?.length).toBe( 2);

      const savedAcme = save.objects?.find((v) => v.i === '101');

      expect(savedAcme?.v).toBe( '1101');
      expect(savedAcme?.roles?.length).toBe( 2);

      const savedAcmeOwner = savedAcme?.roles?.find((v) =>
        info.is(v.t, m.Organisation.Owner)
      );
      const savedAcmeManager = savedAcme?.roles?.find((v) =>
        info.is(v.t, m.Organisation.Manager)
      );

      expect(savedAcmeOwner?.s).toBe( '3');
      expect(savedAcmeOwner?.a).toBeUndefined();
      expect(savedAcmeOwner?.r).toBeUndefined();
      expect(savedAcmeManager?.s).toBe( '2');
      expect(savedAcmeManager?.a).toBeUndefined();
      expect(savedAcmeManager?.r).toBeUndefined();

      const savedOcme = save.objects?.find((v) => v.i === '102');

      expect(savedOcme?.v).toBe( '1102');
      expect(savedOcme?.roles?.length).toBe( 1);

      const savedOcmeOwner = savedOcme?.roles?.find((v) =>
        info.is(v.t, m.Organisation.Owner)
      );

      expect(savedOcmeOwner?.s).toBeNull();
      expect(savedOcmeOwner?.a).toBeUndefined();
      expect(savedOcmeOwner?.r).toBeUndefined();
    });

    it('pushRequest should have all changes from session', () => {
      const koen = session.get('1') as Person;
      const patrick = session.get('2') as Person;
      const martien = session.get('3') as Person;

      const acme = session.get('101') as Organisation;
      const ocme = session.get('102') as Organisation;
      const icme = session.get('103') as Organisation;

      acme.Owner = martien;
      ocme.Owner = null;

      acme.Manager = patrick;

      const save = session.pushRequest();
      const info = new PushRequestInfo(save, m);

      expect(save.objects?.length).toBe( 2);

      const savedAcme = save.objects?.find((v) => v.i === '101');

      expect(savedAcme?.v).toBe( '1101');
      expect(savedAcme?.roles?.length).toBe( 2);

      const savedAcmeOwner = savedAcme?.roles?.find((v) =>
        info.is(v.t, m.Organisation.Owner)
      );
      const savedAcmeManager = savedAcme?.roles?.find((v) =>
        info.is(v.t, m.Organisation.Manager)
      );

      expect(savedAcmeOwner?.s).toBe( '3');
      expect(savedAcmeOwner?.a).toBeUndefined();
      expect(savedAcmeOwner?.r).toBeUndefined();
      expect(savedAcmeManager?.s).toBe( '2');
      expect(savedAcmeManager?.a).toBeUndefined();
      expect(savedAcmeManager?.r).toBeUndefined();

      const savedOcme = save.objects?.find((v) => v.i === '102');

      expect(savedOcme?.v).toBe( '1102');
      expect(savedOcme?.roles?.length).toBe( 1);

      const savedOcmeOwner = savedOcme?.roles?.find((v) =>
        info.is(v.t, m.Organisation.Owner)
      );

      expect(savedOcmeOwner?.s).toBeNull();
      expect(savedOcmeOwner?.a).toBeUndefined();
      expect(savedOcmeOwner?.r).toBeUndefined();
    });

    it('should save with many objects', () => {
      const martien = session.get('3') as Person;

      const mathijs = session.create(m.Person) as Person;
      mathijs.FirstName = 'Mathijs';
      mathijs.LastName = 'Verwer';

      const acme2 = session.create(m.Organisation) as Organisation;
      acme2.Name = 'Acme 2';
      acme2.Manager = mathijs;
      acme2.AddEmployee(mathijs);

      const acme3 = session.create(m.Organisation) as Organisation;
      acme3.Name = 'Acme 3';
      acme3.Manager = martien;
      acme3.AddEmployee(martien);

      const save = session.pushRequest();
      const info = new PushRequestInfo(save, m);

      expect(save.newObjects?.length).toBe( 3);
      expect(save.objects?.length).toBe( 0);

      {
        const savedMathijs = save.newObjects?.find(
          (v) => v.ni === mathijs.newId
        );
        expect(info.is(savedMathijs?.t ?? '', m.Person)).toBeTruthy();
        expect(savedMathijs?.roles?.length).toBe( 2);

        const savedMathijsFirstName = savedMathijs?.roles?.find((v) =>
          info.is(v.t, m.Person.FirstName)
        );
        expect(savedMathijsFirstName?.s).toBe( 'Mathijs');

        const savedMathijsLastName = savedMathijs?.roles?.find((v) =>
          info.is(v.t, m.Person.LastName)
        );
        expect(savedMathijsLastName?.s).toBe( 'Verwer');
      }

      {
        const savedAcme2 = save.newObjects?.find((v) => v.ni === acme2.newId);
        expect(info.is(savedAcme2?.t ?? '', m.Organisation)).toBeTruthy();
        expect(savedAcme2?.roles?.length).toBe( 3);

        const savedAcme2Manager = savedAcme2?.roles?.find((v) =>
          info.is(v.t, m.Organisation.Manager)
        );
        expect(savedAcme2Manager?.s).toBe( mathijs.newId);

        const savedAcme2Employees = savedAcme2?.roles?.find((v) =>
          info.is(v.t, m.Organisation.Employees)
        );
        expect(savedAcme2Employees?.s).toBeUndefined();
        expect(savedAcme2Employees?.a ?? []).toIncludeSameMembers( [mathijs.newId]);
        expect(savedAcme2Employees?.r).toBeUndefined();
      }

      {
        const savedAcme3 = save.newObjects?.find((v) => v.ni === acme3.newId);
        expect(info.is(savedAcme3?.t ?? '', m.Organisation)).toBeTruthy();
        expect(savedAcme3?.roles?.length).toBe( 3);

        const savedAcme3Manager = savedAcme3?.roles?.find((v) =>
          info.is(v.t, m.Organisation.Manager)
        );
        expect(savedAcme3Manager?.s).toBe( '3');

        const savedAcme3Employees = savedAcme3?.roles?.find((v) =>
          info.is(v.t, m.Organisation.Employees)
        );
        expect(savedAcme3Employees?.s).toBeUndefined();
        expect(savedAcme3Employees?.a ?? []).toIncludeSameMembers( ['3']);
        expect(savedAcme3Employees?.r).toBeUndefined();
      }
    });

    it('should save with existing objects', () => {
      const koen = session.get('1') as Person;
      const patrick = session.get('2') as Person;
      const martien = session.get('3') as Person;

      const acme = session.get('101') as Organisation;
      const ocme = session.get('102') as Organisation;
      const icme = session.get('103') as Organisation;

      acme.Employees = [];
      ocme.Employees = [martien, patrick];
      icme.Employees = [koen, patrick, martien];

      const save = session.pushRequest();
      const info = new PushRequestInfo(save, m);

      expect(save.newObjects?.length).toBe( 0);
      expect(save.objects?.length).toBe( 3);

      const savedAcme = save.objects?.find((v) => v.i === '101');

      expect(savedAcme?.v).toBe( '1101');
      expect(savedAcme?.roles?.length).toBe( 1);

      const savedAcmeEmployees = savedAcme?.roles?.find((v) =>
        info.is(v.t, m.Organisation.Employees)
      );

      expect(savedAcmeEmployees?.s).toBeUndefined();
      expect(savedAcmeEmployees?.a ?? []).toIncludeSameMembers( []);
      expect(savedAcmeEmployees?.r ?? []).toIncludeSameMembers( ['1', '2', '3']);

      const savedOcme = save.objects?.find((v) => v.i === '102');

      expect(savedOcme?.v).toBe( '1102');
      expect(savedOcme?.roles?.length).toBe( 1);

      const savedOcmeEmployees = savedOcme?.roles?.find((v) =>
        info.is(v.t, m.Organisation.Employees)
      );

      expect(savedOcmeEmployees?.s).toBeUndefined();
      expect(savedOcmeEmployees?.a ?? []).toIncludeSameMembers( ['2', '3']);
      expect(savedOcmeEmployees?.r ?? []).toIncludeSameMembers( ['1']);

      const savedIcme = save.objects?.find((v) => v.i === '103');

      expect(savedIcme?.v).toBe( '1103');
      expect(savedIcme?.roles?.length).toBe( 1);

      const savedIcmeEmployees = savedIcme?.roles?.find((v) =>
        info.is(v.t, m.Organisation.Employees)
      );

      expect(savedIcmeEmployees?.s).toBeUndefined();
      expect(savedIcmeEmployees?.a ?? []).toIncludeSameMembers( ['1', '2', '3']);
      expect(savedIcmeEmployees?.r).toBeUndefined();
    });

    it('should save with new objects', () => {
      const martien = session.get('3') as Person;

      const mathijs = session.create(m.Person) as Person;
      mathijs.FirstName = 'Mathijs';
      mathijs.LastName = 'Verwer';

      const acme2 = session.create(m.Organisation) as Organisation;
      acme2.Name = 'Acme 2';
      acme2.Manager = mathijs;
      acme2.AddEmployee(mathijs);

      const acme3 = session.create(m.Organisation) as Organisation;
      acme3.Name = 'Acme 3';
      acme3.Manager = martien;
      acme3.AddEmployee(martien);

      const save = session.pushRequest();
      const info = new PushRequestInfo(save, m);

      expect(save.newObjects?.length).toBe( 3);
      expect(save.objects?.length).toBe( 0);

      {
        const savedMathijs = save.newObjects?.find(
          (v) => v.ni === mathijs.newId
        );
        expect(info.is(savedMathijs?.t ?? '', m.Person)).toBeTruthy();
        expect(savedMathijs?.roles?.length).toBe( 2);

        const savedMathijsFirstName = savedMathijs?.roles?.find((v) =>
          info.is(v.t, m.Person.FirstName)
        );
        expect(savedMathijsFirstName?.s).toBe( 'Mathijs');

        const savedMathijsLastName = savedMathijs?.roles?.find((v) =>
          info.is(v.t, m.Person.LastName)
        );
        expect(savedMathijsLastName?.s).toBe( 'Verwer');
      }

      {
        const savedAcme2 = save.newObjects?.find((v) => v.ni === acme2.newId);
        expect(info.is(savedAcme2?.t ?? '', m.Organisation)).toBeTruthy();
        expect(savedAcme2?.roles?.length).toBe( 3);

        const savedAcme2Manager = savedAcme2?.roles?.find((v) =>
          info.is(v.t, m.Organisation.Manager)
        );
        expect(savedAcme2Manager?.s).toBe( mathijs.newId);

        const savedAcme2Employees = savedAcme2?.roles?.find((v) =>
          info.is(v.t, m.Organisation.Employees)
        );
        expect(savedAcme2Employees?.s).toBeUndefined();
        expect(savedAcme2Employees?.a ?? []).toIncludeSameMembers( [mathijs.newId]);
        expect(savedAcme2Employees?.r).toBeUndefined();
      }

      {
        const savedAcme3 = save.newObjects?.find((v) => v.ni === acme3.newId);
        expect(info.is(savedAcme3?.t ?? '', m.Organisation)).toBeTruthy();
        expect(savedAcme3?.roles?.length).toBe( 3);

        const savedAcme3Manager = savedAcme3?.roles?.find((v) =>
          info.is(v.t, m.Organisation.Manager)
        );
        expect(savedAcme3Manager?.s).toBe( '3');

        const savedAcme3Employees = savedAcme3?.roles?.find((v) =>
          info.is(v.t, m.Organisation.Employees)
        );
        expect(savedAcme3Employees?.s).toBeUndefined();
        expect(savedAcme3Employees?.a ?? []).toIncludeSameMembers( ['3']);
        expect(savedAcme3Employees?.r).toBeUndefined();
      }
    });

    it('should exist when created', () => {
      const john = session.create(m.Person) as Person;

      expect(john).toBeDefined();

      expect(john.FirstName).toBeNull();
      expect(john.CanReadFirstName).toBeTruthy();
      expect(john.CanWriteFirstName).toBeTruthy();
    });

    it('reset', () => {
      const martien = session.get('3') as Person;

      const mathijs = session.create(m.Person) as Person;
      mathijs.FirstName = 'Mathijs';
      mathijs.LastName = 'Verwer';

      const acme2 = session.create(m.Organisation) as Organisation;
      acme2.Name = 'Acme 2';
      acme2.Owner = martien;
      acme2.Manager = mathijs;
      acme2.AddEmployee(martien);
      acme2.AddEmployee(mathijs);

      session.reset();

      expect(mathijs.isNew).toBeFalsy();
      expect(mathijs.id).toBeUndefined();
      expect(mathijs.newId).toBeUndefined();
      expect(mathijs.session).toBeUndefined();
      expect(mathijs.objectType).toBeUndefined();
      expect(mathijs.FirstName).toBeUndefined();
      expect(mathijs.LastName).toBeUndefined();
      expect(mathijs.CycleOne).toBeUndefined();
      expect(mathijs.CycleMany).toBeUndefined();

      expect(acme2.isNew).toBeFalsy();
      expect(acme2.id).toBeUndefined();
      expect(acme2.newId).toBeUndefined();
      expect(acme2.session).toBeUndefined();
      expect(acme2.objectType).toBeUndefined();
      expect(acme2.Name).toBeUndefined();
      expect(acme2.Owner).toBeUndefined();
      expect(acme2.Manager).toBeUndefined();
    });

    it('onsaved', () => {
      let pushResponse: PushResponse = {
        hasErrors: false,
        responseType: ResponseType.Push,
      };

      session.pushResponse(pushResponse);

      let mathijs = session.create(m.Person) as Person;
      mathijs.FirstName = 'Mathijs';
      mathijs.LastName = 'Verwer';

      const newId = mathijs.newId;

      pushResponse = {
        hasErrors: false,
        newObjects: [
          {
            i: '10000',
            ni: newId!,
          },
        ],
        responseType: ResponseType.Push,
      };

      session.pushResponse(pushResponse);

      expect(mathijs.newId).toBeUndefined();
      expect(mathijs.id).toBe( '10000');
      expect(mathijs.objectType.name).toBe( 'Person');

      mathijs = session.get('10000') as Person;

      expect(mathijs).toBeDefined();

      let exceptionThrown = false;
      try {
        session.get(newId!);
      } catch (e) {
        exceptionThrown = true;
      }

      expect(exceptionThrown).toBeTruthy();
    });

    it('methodCanExecute', () => {
      const acme = session.get('101') as Organisation;
      const ocme = session.get('102') as Organisation;
      const icme = session.get('103') as Organisation;

      expect(acme.CanReadName).toBeTruthy();
      expect(ocme.CanReadName).toBeFalsy();
      expect(icme.CanReadName).toBeFalsy();

      expect(acme.CanWriteOwner).toBeTruthy();
      expect(ocme.CanWriteOwner).toBeFalsy();
      expect(icme.CanWriteOwner).toBeFalsy();

      expect(acme.CanExecuteJustDoIt).toBeTruthy();
      expect(ocme.CanExecuteJustDoIt).toBeFalsy();
      expect(icme.CanExecuteJustDoIt).toBeFalsy();
    });

    it('get', () => {
      const acme = session.create(m.Organisation) as Organisation;

      let acmeAgain = session.get(acme.id);
      expect(acmeAgain).toBe( acme);

      acmeAgain = session.get(acme.newId!);
      expect(acmeAgain).toBe( acme);
    });

    it('hasChangesWithNewObject', () => {
      expect(session.hasChanges).toBeFalsy();

      const walter = session.create(m.Person) as Person;

      expect(session.hasChanges).toBeTruthy();
    });

    it('hasChangesWithChangedRelations', () => {
      const martien = session.get('3') as Person;

      expect(session.hasChanges).toBeFalsy();

      // Unit
      martien.FirstName = 'New Name';

      expect(session.hasChanges).toBeTruthy();

      session.reset();

      const acme = session.get('101') as Organisation;

      expect(acme.Employees).toContain(martien);

      // Composites
      acme.RemoveEmployee(martien);

      expect(session.hasChanges).toBeTruthy();

      expect(acme.Employees).not.toContain( martien);
    });
  });
});
