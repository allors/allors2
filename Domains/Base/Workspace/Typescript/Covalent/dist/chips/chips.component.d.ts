import { AfterViewInit, EventEmitter, OnDestroy, OnInit } from "@angular/core";
import { NgForm } from "@angular/forms";
import { ISessionObject } from "@allors/base-domain";
import { Observable, Subject, Subscription } from "rxjs";
import { Field } from "@allors/base-angular";
export declare class ChipsComponent extends Field implements OnInit, AfterViewInit, OnDestroy {
    private parentForm;
    display: string;
    debounceTime: number;
    options: ISessionObject[];
    filter: ((search: string) => Observable<ISessionObject[]>);
    onAdd: EventEmitter<ISessionObject>;
    onRemove: EventEmitter<ISessionObject>;
    filteredOptions: ISessionObject[];
    subject: Subject<string>;
    subscription: Subscription;
    private controls;
    constructor(parentForm: NgForm);
    ngOnInit(): void;
    ngAfterViewInit(): void;
    ngOnDestroy(): void;
    add(object: ISessionObject): void;
    remove(object: ISessionObject): void;
    inputChange(search: string): void;
}
