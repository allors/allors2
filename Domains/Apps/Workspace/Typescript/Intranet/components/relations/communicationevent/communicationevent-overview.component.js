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
const material_1 = require("@angular/material");
const router_1 = require("@angular/router");
const core_2 = require("@covalent/core");
const Rx_1 = require("rxjs/Rx");
const workspace_1 = require("@allors/workspace");
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let CommunicationEventOverviewComponent = class CommunicationEventOverviewComponent {
    constructor(workspaceService, errorService, route, dialogService, snackBar, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.route = route;
        this.dialogService = dialogService;
        this.snackBar = snackBar;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Communication Event overview";
        this.scope = this.workspaceService.createScope();
        this.m = this.workspaceService.metaPopulation.metaDomain;
        this.refresh$ = new Rx_1.BehaviorSubject(undefined);
    }
    get isEmail() {
        return this.communicationEventPrefetch instanceof (workspace_1.EmailCommunication);
    }
    get isMeeting() {
        return this.communicationEventPrefetch instanceof (workspace_1.FaceToFaceCommunication);
    }
    get isPhone() {
        return this.communicationEventPrefetch instanceof (workspace_1.PhoneCommunication);
    }
    get isLetter() {
        return this.communicationEvent instanceof (workspace_1.LetterCorrespondence);
    }
    ngOnInit() {
        const route$ = this.route.url;
        const combined$ = Rx_1.Observable.combineLatest(route$, this.refresh$);
        this.subscription = combined$
            .switchMap(([urlSegments, date]) => {
            const id = this.route.snapshot.paramMap.get("id");
            const roleId = this.route.snapshot.paramMap.get("roleId");
            const m = this.m;
            const fetch = [
                new framework_1.Fetch({
                    id: roleId,
                    name: "communicationEventPrefetch",
                }),
                new framework_1.Fetch({
                    name: "party",
                    id,
                }),
            ];
            return this.scope
                .load("Pull", new framework_1.PullRequest({ fetch }))
                .switchMap((loaded) => {
                this.communicationEventPrefetch = loaded.objects.communicationEventPrefetch;
                this.party = loaded.objects.party;
                const fetchEmail = [
                    new framework_1.Fetch({
                        id: roleId,
                        include: [
                            new framework_1.TreeNode({ roleType: m.EmailCommunication.Originator }),
                            new framework_1.TreeNode({ roleType: m.EmailCommunication.Addressees }),
                            new framework_1.TreeNode({ roleType: m.EmailCommunication.EmailTemplate }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                            new framework_1.TreeNode({
                                nodes: [
                                    new framework_1.TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                                    new framework_1.TreeNode({ roleType: m.WorkEffort.Priority }),
                                ],
                                roleType: m.CommunicationEvent.WorkEfforts,
                            }),
                        ],
                        name: "communicationEvent",
                    }),
                ];
                const fetchLetter = [
                    new framework_1.Fetch({
                        id: roleId,
                        include: [
                            new framework_1.TreeNode({ roleType: m.LetterCorrespondence.Originators }),
                            new framework_1.TreeNode({ roleType: m.LetterCorrespondence.Receivers }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                            new framework_1.TreeNode({
                                nodes: [
                                    new framework_1.TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                                    new framework_1.TreeNode({ roleType: m.WorkEffort.Priority }),
                                ],
                                roleType: m.CommunicationEvent.WorkEfforts,
                            }),
                            new framework_1.TreeNode({
                                nodes: [
                                    new framework_1.TreeNode({
                                        nodes: [
                                            new framework_1.TreeNode({ roleType: m.PostalBoundary.Country }),
                                        ],
                                        roleType: m.PostalAddress.PostalBoundary,
                                    }),
                                ],
                                roleType: m.LetterCorrespondence.PostalAddresses,
                            }),
                        ],
                        name: "communicationEvent",
                    }),
                ];
                const fetchMeeting = [
                    new framework_1.Fetch({
                        id: roleId,
                        include: [
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.FromParties }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.ToParties }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                            new framework_1.TreeNode({
                                nodes: [
                                    new framework_1.TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                                    new framework_1.TreeNode({ roleType: m.WorkEffort.Priority }),
                                ],
                                roleType: m.CommunicationEvent.WorkEfforts,
                            }),
                        ],
                        name: "communicationEvent",
                    }),
                ];
                const fetchPhone = [
                    new framework_1.Fetch({
                        id: roleId,
                        include: [
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.FromParties }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.ToParties }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
                            new framework_1.TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                            new framework_1.TreeNode({
                                nodes: [
                                    new framework_1.TreeNode({ roleType: m.WorkEffort.WorkEffortState }),
                                    new framework_1.TreeNode({ roleType: m.WorkEffort.Priority }),
                                ],
                                roleType: m.CommunicationEvent.WorkEfforts,
                            }),
                        ],
                        name: "communicationEvent",
                    }),
                ];
                if (this.isEmail) {
                    return this.scope.load("Pull", new framework_1.PullRequest({ fetch: fetchEmail }));
                }
                if (this.isMeeting) {
                    return this.scope.load("Pull", new framework_1.PullRequest({ fetch: fetchMeeting }));
                }
                if (this.isLetter) {
                    return this.scope.load("Pull", new framework_1.PullRequest({ fetch: fetchLetter }));
                }
                if (this.isPhone) {
                    return this.scope.load("Pull", new framework_1.PullRequest({ fetch: fetchPhone }));
                }
            });
        })
            .subscribe((loaded) => {
            this.scope.session.reset();
            this.communicationEvent = loaded.objects.communicationEvent;
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
        });
    }
    deleteWorkEffort(worktask) {
        this.dialogService
            .openConfirm({ message: "Are you sure you want to delete this work task?" })
            .afterClosed()
            .subscribe((confirm) => {
            if (confirm) {
                this.scope.invoke(worktask.Delete)
                    .subscribe((invoked) => {
                    this.snackBar.open("Successfully deleted.", "close", { duration: 5000 });
                    this.refresh();
                }, (error) => {
                    this.errorService.dialog(error);
                });
            }
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
    refresh() {
        this.refresh$.next(new Date());
    }
    goBack() {
        window.history.back();
    }
};
CommunicationEventOverviewComponent = __decorate([
    core_1.Component({
        templateUrl: "./communicationevent-overview.component.html",
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        router_1.ActivatedRoute,
        core_2.TdDialogService,
        material_1.MatSnackBar,
        core_2.TdMediaService, core_1.ChangeDetectorRef])
], CommunicationEventOverviewComponent);
exports.CommunicationEventOverviewComponent = CommunicationEventOverviewComponent;
