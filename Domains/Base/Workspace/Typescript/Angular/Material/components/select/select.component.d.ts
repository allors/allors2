import { AfterViewInit, EventEmitter } from "@angular/core";
import { NgForm } from "@angular/forms";
import { ISessionObject } from "@allors/framework";
import { Field } from "@allors/base-angular";
export declare class SelectComponent extends Field implements AfterViewInit {
    private parentForm;
    display: string;
    options: ISessionObject[];
    onSelect: EventEmitter<ISessionObject>;
    private controls;
    constructor(parentForm: NgForm);
    ngAfterViewInit(): void;
    selected(option: ISessionObject): void;
}
