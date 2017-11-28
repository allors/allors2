import { AfterViewInit } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Field } from "@allors/base-angular";
export declare class InputComponent extends Field implements AfterViewInit {
    private parentForm;
    private controls;
    constructor(parentForm: NgForm);
    ngAfterViewInit(): void;
}
