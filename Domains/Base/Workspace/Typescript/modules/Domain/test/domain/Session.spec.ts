import { assert } from 'chai';
import { domain, Media, Organisation, Person } from '../../src/allors/domain';
import { MetaPopulation, PushResponse, ResponseType, Session, Workspace, SessionObject } from '../../src/allors/framework';
import { data, MetaDomain } from '../../src/allors/meta';
import { syncResponse } from './fixture';

describe('Session',
    () => {
        let metaPopulation: MetaPopulation;
        let workspace: Workspace;

        beforeEach(() => {
            metaPopulation = new MetaPopulation(data);
            workspace = new Workspace(metaPopulation);
            domain.apply(workspace);
        });

        describe('delete',
        () => {
            let session: Session;

            beforeEach(() => {
                workspace.sync(syncResponse);
                session = new Session(workspace);
            });

            it('should throw exception for existing object', () => {
                const koen = session.get('1') as Person;
                assert.throw( () => { session.delete(koen); } );
            });

            it('should not throw exception for a new object', () => {
                const jos = session.create('Person') as Person;
                assert.doesNotThrow( () => { session.delete(jos); } );
            });

            it('should throw exception for a deleted object', () => {
                const acme = session.create('Organisation') as Organisation;

                session.delete(acme);

                assert.isUndefined(acme.CanReadName);
                assert.isUndefined(acme.CanWriteName);
                assert.isUndefined(acme.Name);
                assert.throw( () => { acme.Name = 'Acme'; } );

                const jos = session.create('Person') as Person;

                assert.isUndefined(acme.CanReadOwner);
                assert.isUndefined(acme.CanWriteOwner);
                assert.isUndefined(acme.Owner);
                assert.throw( () => { acme.Owner = jos; } );

                assert.isUndefined(acme.CanReadEmployees);
                assert.isUndefined(acme.CanWriteEmployees);
                assert.isUndefined(acme.Employees);
                assert.throw( () => { acme.AddEmployee(jos); } );

                assert.isUndefined(acme.CanExecuteJustDoIt);
                assert.isUndefined(acme.JustDoIt);
            });

            it('should delete role from existing object', () => {
                const acme = session.get('101') as Organisation;
                const jos = session.create('Person') as Person;

                acme.Owner = jos;

                session.delete(jos);

                assert.isNull(acme.Owner);
            });

            it('should remove role from existing object', () => {
                const acme = session.get('101') as Organisation;
                const jos = session.create('Person') as Person;

                acme.AddEmployee(jos);

                session.delete(jos);

                assert.notInclude(acme.Employees, jos);
            });

            it('should delete role from new object', () => {
                const acme = session.create('Organisation') as Organisation;
                const jos = session.create('Person') as Person;

                acme.Owner = jos;

                session.delete(jos);

                assert.isNull(acme.Owner);
            });

            it('should remove role from new object', () => {
                const acme = session.create('Organisation') as Organisation;
                const jos = session.create('Person') as Person;

                acme.AddEmployee(jos);

                session.delete(jos);

                assert.notInclude(acme.Employees, jos);
            });
        });

        describe('sync',
        () => {
            let session: Session;

            beforeEach(() => {
                workspace.sync(syncResponse);
                session = new Session(workspace);
            });

            it('should get unit roles', () => {
                const koen = session.get('1') as Person;

                assert.equal(koen.FirstName, 'Koen');
                assert.isNull(koen.MiddleName);
                assert.equal(koen.LastName, 'Van Exem');
                assert.equal(koen.BirthDate.toUTCString(), new Date('1973-03-27T18:00:00Z').toUTCString());
                assert.isTrue(koen.IsStudent);

                const patrick = session.get('2') as Person;

                assert.equal(patrick.FirstName, 'Patrick');
                assert.isNull(patrick.MiddleName, null);
                assert.equal(patrick.LastName, 'De Boeck');
                assert.isNull(patrick.BirthDate);
                assert.isFalse(patrick.IsStudent);

                const martien = session.get('3') as Person;

                assert.equal(martien.FirstName, 'Martien');
                assert.equal(martien.MiddleName, 'van');
                assert.equal(martien.LastName, 'Knippenberg');
                assert.isNull(martien.BirthDate);
                assert.isNull(martien.IsStudent);
            });

            it('should get composite roles', () => {
                const koen = session.get('1') as Person;
                const patrick = session.get('2') as Person;
                const martien = session.get('3') as Person;

                const acme = session.get('101') as Organisation;

                assert.equal(acme.Owner, koen);
                assert.isNull(acme.Manager);

                const ocme = session.get('102') as Organisation;

                assert.equal(ocme.Owner, patrick);
                assert.isNull(ocme.Manager);

                const icme = session.get('103') as Organisation;

                assert.equal(icme.Owner, martien);
                assert.isNull(icme.Manager);
            });

            it('should get composites roles', () => {
                const koen = session.get('1') as Person;
                const patrick = session.get('2') as Person;
                const martien = session.get('3') as Person;

                const acme = session.get('101') as Organisation;
                const ocme = session.get('102') as Organisation;
                const icme = session.get('103') as Organisation;

                assert.sameMembers(acme.Employees, [koen, patrick, martien]);
                assert.sameMembers(ocme.Employees, [koen]);
                assert.sameMembers(icme.Employees, []);

                assert.sameMembers(acme.Shareholders, []);
                assert.sameMembers(ocme.Shareholders, []);
                assert.sameMembers(icme.Shareholders, []);
            });

            describe('two different sessions with same objects',
            () => {
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
                        assert.equal(martien1.FirstName, 'Martien');
                        assert.equal(martien1.LastName, 'Knippenberg');
                        assert.equal(martien1.MiddleName, 'van');
                    });

                    it('should not see changes in the other session', () => {
                        assert.equal(martien2.FirstName, 'Martinus');
                        assert.equal(martien2.LastName, 'Knippenberg');
                        assert.equal(martien2.MiddleName, 'X');
                    });
                });

                describe('update composite roles', () => {

                    beforeEach(() => {
                        acme2.Owner = martien2;
                        ocme2.Owner = null;
                        acme2.Manager = patrick2;
                    });

                    it('should see changes in this session', () => {
                        assert.equal(acme1.Owner, koen1);
                        assert.equal(ocme1.Owner, patrick1);
                        assert.equal(icme1.Owner, martien1);

                        assert.isNull(acme1.Manager);
                        assert.isNull(ocme1.Manager);
                        assert.isNull(icme1.Manager);
                    });

                    it('should not see changes in the other session', () => {
                        assert.equal(acme2.Owner, martien2);
                        assert.isNull(ocme2.Owner);
                        assert.equal(icme2.Owner, martien2);

                        assert.equal(acme2.Manager, patrick2);
                        assert.isNull(ocme2.Manager);
                        assert.isNull(icme2.Manager);
                    });
                });

                describe('update composites roles', () => {

                    beforeEach(() => {
                        acme2.Employees = null;
                        icme2.Employees = [koen2, patrick2, martien2];
                    });

                    it('should see changes in this session', () => {
                        assert.sameMembers(acme1.Employees, [koen1, patrick1, martien1]);
                        assert.sameMembers(ocme1.Employees, [koen1]);
                        assert.sameMembers(icme1.Employees, []);
                    });

                    it('should not see changes in the other session', () => {
                        assert.sameMembers(acme2.Employees, []);
                        assert.sameMembers(ocme2.Employees, [koen2]);
                        assert.sameMembers(icme2.Employees, [koen2, patrick2, martien2]);
                    });
                });
            });

            it('pushRequest should have all changes from session',
            () => {
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

                assert.equal(save.objects.length, 2);

                const savedAcme = save.objects.find((v) => (v.i === '101'));

                assert.equal(savedAcme.v, '1101');
                assert.equal(savedAcme.roles.length, 2);

                const savedAcmeOwner = savedAcme.roles.find((v) => (v.t === 'Owner'));
                const savedAcmeManager = savedAcme.roles.find((v) => (v.t === 'Manager'));

                assert.equal(savedAcmeOwner.s, '3');
                assert.isUndefined(savedAcmeOwner.a);
                assert.isUndefined(savedAcmeOwner.r);
                assert.equal(savedAcmeManager.s, '2');
                assert.isUndefined(savedAcmeManager.a);
                assert.isUndefined(savedAcmeManager.r);

                const savedOcme = save.objects.find((v) => (v.i === '102'));

                assert.equal(savedOcme.v, '1102');
                assert.equal(savedOcme.roles.length, 1);

                const savedOcmeOwner = savedOcme.roles.find((v) => (v.t === 'Owner'));

                assert.isNull(savedOcmeOwner.s);
                assert.isUndefined(savedOcmeOwner.a);
                assert.isUndefined(savedOcmeOwner.r);
            });

            it('pushRequest should have all changes from session',
            () => {
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

                assert.equal(save.objects.length, 2);

                const savedAcme = save.objects.find((v) => (v.i === '101'));

                assert.equal(savedAcme.v, '1101');
                assert.equal(savedAcme.roles.length, 2);

                const savedAcmeOwner = savedAcme.roles.find((v) => (v.t === 'Owner'));
                const savedAcmeManager = savedAcme.roles.find((v) => (v.t === 'Manager'));

                assert.equal(savedAcmeOwner.s, '3');
                assert.isUndefined(savedAcmeOwner.a);
                assert.isUndefined(savedAcmeOwner.r);
                assert.equal(savedAcmeManager.s, '2');
                assert.isUndefined(savedAcmeManager.a);
                assert.isUndefined(savedAcmeManager.r);

                const savedOcme = save.objects.find((v) => (v.i === '102'));

                assert.equal(savedOcme.v, '1102');
                assert.equal(savedOcme.roles.length, 1);

                const savedOcmeOwner = savedOcme.roles.find((v) => (v.t === 'Owner'));

                assert.isNull(savedOcmeOwner.s);
                assert.isUndefined(savedOcmeOwner.a);
                assert.isUndefined(savedOcmeOwner.r);
            });

            it('should save with many objects', () => {
                const martien = session.get('3') as Person;

                const mathijs = session.create('Person') as Person;
                mathijs.FirstName = 'Mathijs';
                mathijs.LastName = 'Verwer';

                const acme2 = session.create('Organisation') as Organisation;
                acme2.Name = 'Acme 2';
                acme2.Manager = mathijs;
                acme2.AddEmployee(mathijs);

                const acme3 = session.create('Organisation') as Organisation;
                acme3.Name = 'Acme 3';
                acme3.Manager = martien;
                acme3.AddEmployee(martien);

                const save = session.pushRequest();
                assert.equal(save.newObjects.length, 3);
                assert.equal(save.objects.length, 0);

                {
                    const savedMathijs = save.newObjects.find((v) => (v.ni === mathijs.newId));
                    assert.equal(savedMathijs.t, 'Person');
                    assert.equal(savedMathijs.roles.length, 2);

                    const savedMathijsFirstName = savedMathijs.roles.find((v) => (v.t === 'FirstName'));
                    assert.equal(savedMathijsFirstName.s, 'Mathijs');

                    const savedMathijsLastName = savedMathijs.roles.find((v) => (v.t === 'LastName'));
                    assert.equal(savedMathijsLastName.s, 'Verwer');
                }

                {
                    const savedAcme2 = save.newObjects.find((v) => (v.ni === acme2.newId));
                    assert.equal(savedAcme2.t, 'Organisation');
                    assert.equal(savedAcme2.roles.length, 3);

                    const savedAcme2Manager = savedAcme2.roles.find((v) => (v.t === 'Manager'));
                    assert.equal(savedAcme2Manager.s, mathijs.newId);

                    const savedAcme2Employees = savedAcme2.roles.find((v) => (v.t === 'Employees'));
                    assert.isUndefined(savedAcme2Employees.s);
                    assert.sameMembers(savedAcme2Employees.a, [mathijs.newId]);
                    assert.isUndefined(savedAcme2Employees.r);
                }

                {
                    const savedAcme3 = save.newObjects.find((v) => (v.ni === acme3.newId));
                    assert.equal(savedAcme3.t, 'Organisation');
                    assert.equal(savedAcme3.roles.length, 3);

                    const savedAcme3Manager = savedAcme3.roles.find((v) => (v.t === 'Manager'));
                    assert.equal(savedAcme3Manager.s, '3');

                    const savedAcme3Employees = savedAcme3.roles.find((v) => (v.t === 'Employees'));
                    assert.isUndefined(savedAcme3Employees.s);
                    assert.sameMembers(savedAcme3Employees.a, ['3']);
                    assert.isUndefined(savedAcme3Employees.r);
                }
            });

            it('should save with existing objects', () => {
                const koen = session.get('1') as Person;
                const patrick = session.get('2') as Person;
                const martien = session.get('3') as Person;

                const acme = session.get('101') as Organisation;
                const ocme = session.get('102') as Organisation;
                const icme = session.get('103') as Organisation;

                acme.Employees = null;
                ocme.Employees = [martien, patrick];
                icme.Employees = [koen, patrick, martien];

                const save = session.pushRequest();

                assert.equal(save.newObjects.length, 0);
                assert.equal(save.objects.length, 3);

                const savedAcme = save.objects.find((v) => (v.i === '101'));

                assert.equal(savedAcme.v, '1101');
                assert.equal(savedAcme.roles.length, 1);

                const savedAcmeEmployees = savedAcme.roles.find((v) => (v.t === 'Employees'));

                assert.isUndefined(savedAcmeEmployees.s);
                assert.sameMembers(savedAcmeEmployees.a, []);
                assert.sameMembers(savedAcmeEmployees.r, ['1', '2', '3']);

                const savedOcme = save.objects.find((v) => (v.i === '102'));

                assert.equal(savedOcme.v, '1102');
                assert.equal(savedOcme.roles.length, 1);

                const savedOcmeEmployees = savedOcme.roles.find((v) => (v.t === 'Employees'));

                assert.isUndefined(savedOcmeEmployees.s);
                assert.sameMembers(savedOcmeEmployees.a, ['2', '3']);
                assert.sameMembers(savedOcmeEmployees.r, ['1']);

                const savedIcme = save.objects.find((v) => (v.i === '103'));

                assert.equal(savedIcme.v, '1103');
                assert.equal(savedIcme.roles.length, 1);

                const savedIcmeEmployees = savedIcme.roles.find((v) => (v.t === 'Employees'));

                assert.isUndefined(savedIcmeEmployees.s);
                assert.sameMembers(savedIcmeEmployees.a, ['1', '2', '3']);
                assert.isUndefined(savedIcmeEmployees.r);
            });

            it('should save with new objects', () => {

                const martien = session.get('3') as Person;

                const mathijs = session.create('Person') as Person;
                mathijs.FirstName = 'Mathijs';
                mathijs.LastName = 'Verwer';

                const acme2 = session.create('Organisation') as Organisation;
                acme2.Name = 'Acme 2';
                acme2.Manager = mathijs;
                acme2.AddEmployee(mathijs);

                const acme3 = session.create('Organisation') as Organisation;
                acme3.Name = 'Acme 3';
                acme3.Manager = martien;
                acme3.AddEmployee(martien);

                const save = session.pushRequest();

                assert.equal(save.newObjects.length, 3);
                assert.equal(save.objects.length, 0);

                {
                    const savedMathijs = save.newObjects.find((v) => (v.ni === mathijs.newId));
                    assert.equal(savedMathijs.t, 'Person');
                    assert.equal(savedMathijs.roles.length, 2);

                    const savedMathijsFirstName = savedMathijs.roles.find((v) => (v.t === 'FirstName'));
                    assert.equal(savedMathijsFirstName.s, 'Mathijs');

                    const savedMathijsLastName = savedMathijs.roles.find((v) => (v.t === 'LastName'));
                    assert.equal(savedMathijsLastName.s, 'Verwer');
                }

                {
                    const savedAcme2 = save.newObjects.find((v) => (v.ni === acme2.newId));
                    assert.equal(savedAcme2.t, 'Organisation');
                    assert.equal(savedAcme2.roles.length, 3);

                    const savedAcme2Manager = savedAcme2.roles.find((v) => (v.t === 'Manager'));
                    assert.equal(savedAcme2Manager.s, mathijs.newId);

                    const savedAcme2Employees = savedAcme2.roles.find((v) => (v.t === 'Employees'));
                    assert.isUndefined(savedAcme2Employees.s);
                    assert.sameMembers(savedAcme2Employees.a, [mathijs.newId]);
                    assert.isUndefined(savedAcme2Employees.r);
                }

                {
                    const savedAcme3 = save.newObjects.find((v) => (v.ni === acme3.newId));
                    assert.equal(savedAcme3.t, 'Organisation');
                    assert.equal(savedAcme3.roles.length, 3);

                    const savedAcme3Manager = savedAcme3.roles.find((v) => (v.t === 'Manager'));
                    assert.equal(savedAcme3Manager.s, '3');

                    const savedAcme3Employees = savedAcme3.roles.find((v) => (v.t === 'Employees'));
                    assert.isUndefined(savedAcme3Employees.s);
                    assert.sameMembers(savedAcme3Employees.a, ['3']);
                    assert.isUndefined(savedAcme3Employees.r);
                }
            });

            it('should exist when created', () => {

                const john = session.create('Person') as Person;

                assert.exists(john);

                assert.isNull(john.FirstName);
                assert.isTrue(john.CanReadFirstName);
                assert.isTrue(john.CanWriteFirstName);
            });

            it('reset', () => {

                const martien = session.get('3') as Person;

                const mathijs = session.create('Person') as Person;
                mathijs.FirstName = 'Mathijs';
                mathijs.LastName = 'Verwer';

                const acme2 = session.create('Organisation') as Organisation;
                acme2.Name = 'Acme 2';
                acme2.Owner = martien;
                acme2.Manager = mathijs;
                acme2.AddEmployee(martien);
                acme2.AddEmployee(mathijs);

                session.reset();

                assert.isFalse(mathijs.isNew);
                assert.isUndefined(mathijs.id);
                assert.isUndefined(mathijs.newId);
                assert.isUndefined(mathijs.session);
                assert.isUndefined(mathijs.objectType);
                assert.isUndefined(mathijs.FirstName);
                assert.isUndefined(mathijs.LastName);
                assert.isUndefined(mathijs.CycleOne);
                assert.isUndefined(mathijs.CycleMany);

                assert.isFalse(acme2.isNew);
                assert.isUndefined(acme2.id);
                assert.isUndefined(acme2.newId);
                assert.isUndefined(acme2.session);
                assert.isUndefined(acme2.objectType);
                assert.isUndefined(acme2.Name);
                assert.isUndefined(acme2.Owner);
                assert.isUndefined(acme2.Manager);
            });

            it('onsaved', () => {
                let saveResponse: PushResponse = {
                    hasErrors: false,
                    responseType: ResponseType.Push,
                };

                session.pushResponse(saveResponse);

                let mathijs = session.create('Person') as Person;
                mathijs.FirstName = 'Mathijs';
                mathijs.LastName = 'Verwer';

                const newId = mathijs.newId;

                saveResponse = {
                    hasErrors: false,
                    newObjects: [
                        {
                            i: '10000',
                            ni: newId,
                        },
                    ],
                    responseType: ResponseType.Push,
                };

                session.pushResponse(saveResponse);

                assert.isUndefined(mathijs.newId);
                assert.equal(mathijs.id, '10000');
                assert.equal(mathijs.objectType.name, 'Person');

                mathijs = session.get('10000') as Person;

                assert.exists(mathijs);

                let exceptionThrown = false;
                try {
                    session.get(newId);
                } catch (e) {
                    exceptionThrown = true;
                }

                assert.isTrue(exceptionThrown);
            });

            it('methodCanExecute', () => {
                const acme = session.get('101') as Organisation;
                const ocme = session.get('102') as Organisation;
                const icme = session.get('103') as Organisation;

                assert.isTrue(acme.CanExecuteJustDoIt);
                assert.isFalse(ocme.CanExecuteJustDoIt);
                assert.isFalse(icme.CanExecuteJustDoIt);
            });

            it('get', () => {
                const acme = session.create('Organisation') as Organisation;

                let acmeAgain = session.get(acme.id);
                assert.equal(acmeAgain, acme);

                acmeAgain = session.get(acme.newId);
                assert.equal(acmeAgain, acme);
            });

            it('hasChangesWithNewObject', () => {
                assert.isFalse(session.hasChanges);

                const walter = session.create('Person') as Person;

                assert.isTrue(session.hasChanges);
            });

            it('hasChangesWithChangedRelations', () => {
                const martien = session.get('3') as Person;

                assert.isFalse(session.hasChanges);

                // Unit
                martien.FirstName = 'New Name';

                assert.isTrue(session.hasChanges);

                session.reset();

                const acme = session.get('101') as Organisation;

                assert.include(acme.Employees, martien);

                // Composites
                acme.RemoveEmployee(martien);

                assert.isTrue(session.hasChanges);

                assert.notInclude(acme.Employees, martien);
            });
        });
});
