import { ResponseType, SyncResponse, SecurityResponse, Operations } from "@allors/protocol/system";
import { Meta } from "@allors/meta/generated";

export function syncResponse(m: Meta): SyncResponse {
  return {
    responseType: ResponseType.Sync,
    hasErrors: false,
    objects: [
      {
        i: "1",
        t: m.Person.id,
        v: "1001",
        r: [
          { t: m.Person.FirstName.id, v: "Koen" },
          { t: m.Person.LastName.id, v: "Van Exem" },
          { t: m.Person.BirthDate.id, v: "1973-03-27T18:00:00Z" },
          { t: m.Person.IsStudent.id, v: "true" },
        ],
      },
      {
        i: "2",
        t: m.Person.id,
        v: "1002",
        r: [
          { t: m.Person.FirstName.id, v: "Patrick" },
          { t: m.Person.LastName.id, v: "De Boeck" },
          { t: m.Person.IsStudent.id, v: "false" },
        ],
      },
      {
        i: "3",
        t: m.Person.id,
        v: "1003",
        r: [
          { t: m.Person.FirstName.id, v: "Martien" },
          { t: m.Person.MiddleName.id, v: "van" },
          { t: m.Person.LastName.id, v: "Knippenberg" },
        ],
      },
      {
        i: "101",
        t: m.Organisation.id,
        v: "1101",
        r: [
          { t: m.Organisation.Name.id, v: "Acme" },
          { t: m.Organisation.Owner.id, v: "1" },
          { t: m.Organisation.Employees.id, v: "1|2|3" },
        ],
        a: "801",
      },
      {
        i: "102",
        t: m.Organisation.id,
        v: "1102",
        r: [
          { t: m.Organisation.Name.id, v: "Ocme" },
          { t: m.Organisation.Owner.id, v: "2" },
          { t: m.Organisation.Employees.id, v: "1" },
        ],
      },
      {
        i: "103",
        t: m.Organisation.id,
        v: "1103",
        r: [
          { t: m.Organisation.Name.id, v: "icme" },
          { t: m.Organisation.Owner.id, v: "3" },
        ],
      },
    ],
    accessControls: [["801", "1"]],
  };
}

export function securityResponse(m: Meta): SecurityResponse {
  return {
    responseType: ResponseType.Security,
    hasErrors: false,
    accessControls: [
      {
        i: "801",
        v: "1",
        p: "901,902,903",
      },
    ],
    permissions: [
      [
        "901",
        m.Organisation.id,
        m.Organisation.JustDoIt.id,
        Operations.Execute.toString(),
      ],
      [
        "902",
        m.Organisation.id,
        m.Organisation.Name.id,
        Operations.Read.toString(),
      ],
      [
        "903",
        m.Organisation.id,
        m.Organisation.Owner.id,
        Operations.Write.toString(),
      ],
    ],
  };
}

export function securityResponse2(m: Meta): SecurityResponse {
  return {
    responseType: ResponseType.Security,
    hasErrors: false,
    accessControls: [
      {
        i: "802",
        v: "1",
        p: "902,903",
      },
    ],
    permissions: [
      [
        "902",
        m.Organisation.id,
        m.Organisation.Name.id,
        Operations.Read.toString(),
      ],
      [
        "903",
        m.Organisation.id,
        m.Organisation.Owner.id,
        Operations.Write.toString(),
      ],
    ],
  };
}
