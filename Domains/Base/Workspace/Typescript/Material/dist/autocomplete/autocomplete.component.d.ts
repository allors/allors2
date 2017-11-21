import { EventEmitter, OnInit } from "@angular/core";
import { FormControl } from "@angular/forms";
import { Observable } from "rxjs";
import { Field } from "@allors/base-angular";
import { ISessionObject } from "@allors/base-domain";
export declare class AutocompleteComponent extends Field implements OnInit {
    display: string;
    debounceTime: number;
    options: ISessionObject[];
    filter: ((search: string) => Observable<ISessionObject[]>);
    onSelect: EventEmitter<ISessionObject>;
    filteredOptions: Observable<ISessionObject[]>;
    searchControl: FormControl;
    ngOnInit(): void;
    displayFn(): (val: ISessionObject) => string;
    selected(option: ISessionObject): void;
    focusout(): void;
}
