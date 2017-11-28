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
const forms_1 = require("@angular/forms");
const material_1 = require("@angular/material");
const platform_browser_1 = require("@angular/platform-browser");
const router_1 = require("@angular/router");
const Rx_1 = require("rxjs/Rx");
const core_2 = require("@covalent/core");
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let OrganisationsOverviewComponent = class OrganisationsOverviewComponent {
    constructor(workspaceService, errorService, formBuilder, titleService, snackBar, router, dialogService, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.formBuilder = formBuilder;
        this.titleService = titleService;
        this.snackBar = snackBar;
        this.router = router;
        this.dialogService = dialogService;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Organisations";
        this.scope = this.workspaceService.createScope();
        this.refresh$ = new Rx_1.BehaviorSubject(undefined);
        this.searchForm = this.formBuilder.group({
            name: [""],
            country: [""],
            role: [""],
            classification: [""],
            contactFirstName: [""],
            contactLastName: [""],
        });
        this.page$ = new Rx_1.BehaviorSubject(50);
        const search$ = this.searchForm.valueChanges
            .debounceTime(400)
            .distinctUntilChanged()
            .startWith({});
        const combined$ = Rx_1.Observable
            .combineLatest(search$, this.page$, this.refresh$)
            .scan(([previousData, previousTake, previousDate], [data, take, date]) => {
            return [
                data,
                data !== previousData ? 50 : take,
                date,
            ];
        }, []);
        this.subscription = combined$
            .switchMap(([data, take]) => {
            const m = this.workspaceService.metaPopulation.metaDomain;
            const organisationRolesQuery = [
                new framework_1.Query({
                    name: "organisationRoles",
                    objectType: m.OrganisationRole,
                }),
                new framework_1.Query({
                    name: "classifications",
                    objectType: m.CustomOrganisationClassification,
                }),
                new framework_1.Query({
                    name: "countries",
                    objectType: m.Country,
                    sort: [new framework_1.Sort({ roleType: m.Country.Name })],
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ query: organisationRolesQuery }))
                .switchMap((loaded) => {
                this.roles = loaded.collections.organisationRoles;
                this.role = this.roles.find((v) => v.Name === data.role);
                this.countries = loaded.collections.countries;
                this.country = this.countries.find((v) => v.Name === data.country);
                this.classifications = loaded.collections.classifications;
                this.classification = this.classifications.find((v) => v.Name === data.classification);
                const contactPredicate = new framework_1.And();
                const contactPredicates = contactPredicate.predicates;
                if (data.contactFirstName) {
                    const like = "%" + data.contactFirstName + "%";
                    contactPredicates.push(new framework_1.Like({ roleType: m.Person.FirstName, value: like }));
                }
                if (data.contactLastName) {
                    const like = "%" + data.contactLastName + "%";
                    contactPredicates.push(new framework_1.Like({ roleType: m.Person.LastName, value: like }));
                }
                const contactQuery = new framework_1.Query({
                    name: "contacts",
                    objectType: m.Person,
                    predicate: contactPredicate,
                });
                const organisationContactRelationshipPredicate = new framework_1.And();
                const organisationContactRelationshipPredicates = organisationContactRelationshipPredicate.predicates;
                organisationContactRelationshipPredicates.push(new framework_1.ContainedIn({ roleType: m.OrganisationContactRelationship.Contact, query: contactQuery }));
                const organisationContactRelationshipQuery = new framework_1.Query({
                    name: "organisationContactRelationships",
                    objectType: m.OrganisationContactRelationship,
                    predicate: organisationContactRelationshipPredicate,
                });
                const postalBoundaryPredicate = new framework_1.And();
                const postalBoundaryPredicates = postalBoundaryPredicate.predicates;
                if (data.country) {
                    postalBoundaryPredicates.push(new framework_1.Equals({ roleType: m.PostalBoundary.Country, value: this.country }));
                }
                const postalBoundaryQuery = new framework_1.Query({
                    name: "postalBoundaries",
                    objectType: m.PostalBoundary,
                    predicate: postalBoundaryPredicate,
                });
                const postalAddressPredicate = new framework_1.And();
                const postalAddressPredicates = postalAddressPredicate.predicates;
                postalAddressPredicates.push(new framework_1.ContainedIn({ roleType: m.PostalAddress.PostalBoundary, query: postalBoundaryQuery }));
                const postalAddressQuery = new framework_1.Query({
                    name: "postalAddresses",
                    objectType: m.PostalAddress,
                    predicate: postalAddressPredicate,
                });
                const predicate = new framework_1.And();
                const predicates = predicate.predicates;
                if (data.role) {
                    predicates.push(new framework_1.Contains({ roleType: m.Organisation.OrganisationRoles, object: this.role }));
                }
                if (data.classification) {
                    predicates.push(new framework_1.Contains({ roleType: m.Organisation.OrganisationClassifications, object: this.classification }));
                }
                if (data.country) {
                    predicates.push(new framework_1.ContainedIn({ roleType: m.Organisation.GeneralCorrespondence, query: postalAddressQuery }));
                }
                if (data.contactFirstName || data.contactLastName) {
                    predicates.push(new framework_1.ContainedIn({ roleType: m.Organisation.CurrentOrganisationContactRelationships, query: organisationContactRelationshipQuery }));
                }
                if (data.name) {
                    const like = data.name.replace("*", "%") + "%";
                    predicates.push(new framework_1.Like({ roleType: m.Organisation.Name, value: like }));
                }
                const query = [new framework_1.Query({
                        name: "organisations",
                        objectType: m.Organisation,
                        predicate,
                        page: new framework_1.Page({ skip: 0, take: take }),
                        include: [
                            new framework_1.TreeNode({ roleType: m.Organisation.OrganisationClassifications }),
                            new framework_1.TreeNode({ roleType: m.Organisation.GeneralPhoneNumber }),
                            new framework_1.TreeNode({
                                roleType: m.Organisation.GeneralCorrespondence,
                                nodes: [
                                    new framework_1.TreeNode({
                                        roleType: m.PostalAddress.PostalBoundary,
                                        nodes: [
                                            new framework_1.TreeNode({ roleType: m.PostalBoundary.Country }),
                                        ],
                                    }),
                                ],
                            }),
                        ],
                        sort: [new framework_1.Sort({ roleType: m.Organisation.Name })],
                    })];
                return this.scope
                    .load("Pull", new framework_1.PullRequest({ query }));
            });
        })
            .subscribe((loaded) => {
            this.scope.session.reset();
            this.data = loaded.collections.organisations;
            this.total = loaded.values.organisations_total;
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
        });
    }
    more() {
        this.page$.next(this.data.length + 50);
    }
    goBack() {
        window.history.back();
    }
    ngAfterViewInit() {
        this.titleService.setTitle("Organisations");
        this.media.broadcast();
        this.changeDetectorRef.detectChanges();
    }
    ngOnDestroy() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }
    refresh() {
        this.refresh$.next(new Date());
    }
    delete(organisation) {
        this.dialogService
            .openConfirm({ message: "Are you sure you want to delete this organisation?" })
            .afterClosed()
            .subscribe((confirm) => {
            if (confirm) {
                this.scope.invoke(organisation.Delete)
                    .subscribe((invoked) => {
                    this.snackBar.open("Successfully deleted.", "close", { duration: 5000 });
                    this.refresh();
                }, (error) => {
                    this.errorService.dialog(error);
                });
            }
        });
    }
    onView(organisation) {
        this.router.navigate(["/relations/organisation/" + organisation.id]);
    }
};
OrganisationsOverviewComponent = __decorate([
    core_1.Component({
        templateUrl: "./organisations-overview.component.html",
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        forms_1.FormBuilder,
        platform_browser_1.Title,
        material_1.MatSnackBar,
        router_1.Router,
        core_2.TdDialogService,
        core_2.TdMediaService, core_1.ChangeDetectorRef])
], OrganisationsOverviewComponent);
exports.OrganisationsOverviewComponent = OrganisationsOverviewComponent;
