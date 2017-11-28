"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const router_1 = require("@angular/router");
const core_2 = require("@covalent/core");
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let InvoicePrintComponent = class InvoicePrintComponent {
    constructor(workspaceService, errorService, route, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.route = route;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.scope = this.workspaceService.createScope();
        this.m = this.workspaceService.metaPopulation.metaDomain;
    }
    ngOnInit() {
        this.subscription = this.route.url
            .switchMap((url) => {
            const id = this.route.snapshot.paramMap.get("id");
            const m = this.m;
            const fetch = [
                new framework_1.Fetch({
                    id,
                    name: "invoice",
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ fetch }));
        })
            .subscribe((loaded) => {
            this.invoice = loaded.objects.invoice;
            const printContent = this.invoice.PrintContent;
            const wrapper = document.createElement("div");
            wrapper.innerHTML = printContent;
            this.body = wrapper.querySelector("#dataContainer").innerHTML;
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
        });
    }
    ngAfterViewInit() {
        this.media.broadcast();
        this.changeDetectorRef.detectChanges();
    }
    ngOnDestroy() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }
    goBack() {
        window.history.back();
    }
};
InvoicePrintComponent = __decorate([
    core_1.Component({
        encapsulation: core_1.ViewEncapsulation.Native,
        template: `<div [innerHTML]="body"></div>`,
        styles: [`
invoice-box {
    max-width: 800px;
    min-width: 800px;
    margin: auto;
    padding: 50px 30px 30px 30px;
    font-size: 13px;
    font-weight: normal;
    line-height: 13px;
    font-family:"Arial Rounded MT Bold", "Helvetica Rounded", Arial, sans-serif;
    color: #555;
}

    .invoice-box table {
        width: 100%;
        line-height: inherit;
        text-align: left;
    }

tr.ruler-top-normal td {
    border-top: 1px solid #000;
}

tr.ruler-bottom-normal td {
    border-bottom: 1px solid #000;
}

tr.ruler-top td {
    border-top: 2px solid #000;
}

tr.ruler-bottom td {
    border-bottom: 2px solid #000;
}

.invoice-box table td {
    padding-top: 2px;
    vertical-align: top;
}

    .invoice-box table td.amount {
        text-align: right;
    }

tr.top table tr td.title {
    font-size: 24px;
    font-weight: bold;
    vertical-align: middle;
    text-align: left;
    color: #000;
}

.invoice-box table tr.top table tr td.number {
    font-size: 20px;
    font-weight: bold;
    vertical-align: middle;
    text-align: right;
}

.invoice-box table tr.header td {
    padding-top: 5px;
    padding-bottom: 5px;
    padding-right: 5px;
    font-weight: bold;
    vertical-align: middle;
    text-align: left;
}

.invoice-box table tr.item td {
    padding-top: 5px;
    padding-right: 5px;
}

.headerSpacer {
    height: 20px;
}

.logo {
    max-width: 200px;
    float: left;
}

.description {
    padding-top: 30px;
    padding-bottom: 15px;
    font-weight: bold;
}

.totals {
    float: right;
    width: 35%;
    position: relative;
    top: 100px;
}

.bold {
    font-weight: bold;
}

.footer {
    position: absolute;
    bottom: 10px;
}

    .footer p {
        margin: 3px;
        font-size: 8px;
        line-height: 10px;
        text-align: left;
        font-weight: normal;
    }

@media print {
  @page {
      size: auto; /* auto is the initial value */
      margin: 0; /* this affects the margin in the printer settings */
  }
}
`]
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        router_1.ActivatedRoute,
        core_2.TdMediaService, core_1.ChangeDetectorRef])
], InvoicePrintComponent);
exports.InvoicePrintComponent = InvoicePrintComponent;
