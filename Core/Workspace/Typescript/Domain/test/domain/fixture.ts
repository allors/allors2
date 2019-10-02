import { Meta } from '../../src/allors/meta';
import { SyncResponse } from '../../src/allors/framework/protocol/sync/SyncResponse';
import { Compressor } from '../../src/allors/framework/protocol/Compressor';
import { ResponseType } from '../../src/allors/framework/protocol/ResponseType';
import { SecurityResponse } from '../../src/allors/framework/protocol/security/SecurityResponse';
import { Operations } from '../../src/allors/framework/protocol/Operations';

export function syncResponse(m: Meta): SyncResponse {

  const c = new Compressor();

  return {
    responseType: ResponseType.Sync,
    hasErrors: false,
    objects: [
      {
        i: '1',
        t: c.write(m.Person.id),
        v: '1001',
        r: [
          { t: c.write(m.Person.FirstName.id), v: 'Koen' },
          { t: c.write(m.Person.LastName.id), v: 'Van Exem' },
          { t: c.write(m.Person.BirthDate.id), v: '1973-03-27T18:00:00Z' },
          { t: c.write(m.Person.IsStudent.id), v: 'true' },
        ],
      },
      {
        i: '2',
        t: c.write(m.Person.id),
        v: '1002',
        r: [
          { t: c.write(m.Person.FirstName.id), v: 'Patrick' },
          { t: c.write(m.Person.LastName.id), v: 'De Boeck' },
          { t: c.write(m.Person.IsStudent.id), v: 'false' },
        ],
      },
      {
        i: '3',
        t: c.write(m.Person.id),
        v: '1003',
        r: [
          { t: c.write(m.Person.FirstName.id), v: 'Martien' },
          { t: c.write(m.Person.MiddleName.id), v: 'van' },
          { t: c.write(m.Person.LastName.id), v: 'Knippenberg' },
        ],
      },
      {
        i: '101',
        t: c.write(m.Organisation.id),
        v: '1101',
        r: [
          { t: c.write(m.Organisation.Name.id), v: 'Acme' },
          { t: c.write(m.Organisation.Owner.id), v: '1' },
          { t: c.write(m.Organisation.Employees.id), v: '1,2,3' },
        ],
        a: c.write('801'),
      },
      {
        i: '102',
        t: c.write(m.Organisation.id),
        v: '1102',
        r: [
          { t: c.write(m.Organisation.Name.id), v: 'Ocme' },
          { t: c.write(m.Organisation.Owner.id), v: '2' },
          { t: c.write(m.Organisation.Employees.id), v: '1' },
        ],
      },
      {
        i: '103',
        t: c.write(m.Organisation.id),
        v: '1103',
        r: [
          { t: c.write(m.Organisation.Name.id), v: 'icme' },
          { t: c.write(m.Organisation.Owner.id), v: '3' },
        ],
      },
    ],
    accessControls: [
      ['801', '1']
    ]
  }
}

export function securityResponse(m: Meta): SecurityResponse {

  const c = new Compressor();

  return {
    responseType: ResponseType.Security,
    hasErrors: false,
    accessControls: [{
      i: '801',
      v: '1',
      p: ['901', '902', '903']
    }],
    permissions: [
      ['901', c.write(m.Organisation.id), c.write(m.Organisation.JustDoIt.id), Operations.Execute.toString()],
      ['902', c.write(m.Organisation.id), c.write(m.Organisation.Name.id), Operations.Read.toString()],
      ['903', c.write(m.Organisation.id), c.write(m.Organisation.Owner.id), Operations.Write.toString()],
    ]
  }
}
