import { AfterViewInit, QueryList } from "@angular/core";
import { NgForm, NgModel } from "@angular/forms";
import { Field } from "@allors/base-angular";
export declare class DatepickerComponent extends Field implements AfterViewInit {
    private parentForm;
    useTime: boolean;
    controls: QueryList<NgModel>;
    constructor(parentForm: NgForm);
    ngAfterViewInit(): void;
    hours: number;
    minutes: number;
}
