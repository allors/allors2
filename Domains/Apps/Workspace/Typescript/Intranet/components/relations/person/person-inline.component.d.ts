import { EventEmitter, OnInit } from "@angular/core";
import { MetaDomain, Locale, Enumeration, Person, PersonRole } from "@allors/workspace";
import { WorkspaceService, ErrorService } from "@allors/base-angular";
export declare class PersonInlineComponent implements OnInit {
    private workspaceService;
    private errorService;
    saved: EventEmitter<string>;
    cancelled: EventEmitter<any>;
    person: Person;
    m: MetaDomain;
    locales: Locale[];
    genders: Enumeration[];
    salutations: Enumeration[];
    roles: PersonRole[];
    private scope;
    constructor(workspaceService: WorkspaceService, errorService: ErrorService);
    ngOnInit(): void;
    cancel(): void;
    save(): void;
}
