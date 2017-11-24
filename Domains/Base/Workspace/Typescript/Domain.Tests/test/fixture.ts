import { ResponseType, SyncResponse  } from "../src/index";

export let syncResponse: SyncResponse = {
    hasErrors: false,
    objects: [
        {
            i: "1",
            roles: [
                ["FirstName", "rw", "Koen"],
                ["LastName", "rw", "Van Exem"],
                ["BirthDate", "rw", "1973-03-27T18:00:00Z"],
                ["IsStudent", "rw", true],
            ],
            t: "Person",
            v: "1001",
        },
        {
            i: "2",
            roles: [
                ["FirstName", "rw", "Patrick"],
                ["LastName", "rw", "De Boeck"],
                ["IsStudent", "rw", false],
            ],
            t: "Person",
            v: "1002",
        },
        {
            i: "3",
            roles: [
                ["FirstName", "rw", "Martien"],
                ["MiddleName", "rw", "van"],
                ["LastName", "rw", "Knippenberg"],
            ],
            t: "Person",
            v: "1003",
        },
        {
            i: "101",
            methods: [
                ["JustDoIt", "x"],
            ],
            roles: [
                ["Name", "rw", "Acme"],
                ["Owner", "rw", "1"],
                ["Employees", "rw", ["1", "2", "3"]],
            ],
            t: "Organisation",
            v: "1101",
        },
        {
            i: "102",
            methods: [
                ["JustDoIt", ""],
            ],
            roles: [
                ["Name", "rw", "Ocme"],
                ["Owner", "rw", "2"],
                ["Employees", "rw", ["1"]],
            ],
            t: "Organisation",
            v: "1102",
        },
        {
            i: "103",
            methods: [
                ["JustDoIt", ""],
            ],
            roles: [
                ["Name", "rw", "icme"],
                ["Owner", "rw", "3"],
            ],
            t: "Organisation",
            v: "1103",
        },
    ],
    responseType: ResponseType.Sync,
    userSecurityHash: "#",
};
