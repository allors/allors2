"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var http_1 = require("@angular/common/http");
var core_1 = require("@angular/core");
var forms_1 = require("@angular/forms");
var platform_browser_1 = require("@angular/platform-browser");
var animations_1 = require("@angular/platform-browser/animations");
var app_routing_module_1 = require("./app-routing.module");
var app_component_1 = require("./app.component");
var environment_1 = require("../environments/environment");
var authorization_service_1 = require("./auth/authorization.service");
var login_component_1 = require("./auth/login.component");
var dashboard_component_1 = require("./dashboard/dashboard.component");
var form_component_1 = require("./form/form.component");
var material_1 = require("@angular/material");
var MATERIAL_MODULES = [
    material_1.MatAutocompleteModule, material_1.MatButtonModule, material_1.MatCardModule, material_1.MatCheckboxModule, material_1.MatDatepickerModule,
    material_1.MatIconModule, material_1.MatInputModule, material_1.MatListModule, material_1.MatMenuModule,
    material_1.MatNativeDateModule, material_1.MatRadioModule, material_1.MatSelectModule,
    material_1.MatSidenavModule, material_1.MatSliderModule, material_1.MatSlideToggleModule,
    material_1.MatSnackBarModule, material_1.MatTabsModule, material_1.MatToolbarModule, material_1.MatTooltipModule,
];
var base_angular_1 = require("@allors/base-angular");
var base_material_1 = require("@allors/base-material");
var BASE_MATERIAL_MODULES = [
    base_material_1.AutoCompleteModule, base_material_1.CheckboxModule, base_material_1.DatepickerModule, base_material_1.InputModule, base_material_1.LocalisedTextModule,
    base_material_1.RadioGroupModule, base_material_1.SelectModule, base_material_1.SliderModule, base_material_1.SlideToggleModule, base_material_1.StaticModule, base_material_1.TextAreaModule,
];
var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            bootstrap: [app_component_1.AppComponent],
            declarations: [
                app_component_1.AppComponent,
                login_component_1.LoginComponent,
                dashboard_component_1.DashboardComponent,
                form_component_1.FormComponent,
            ],
            imports: [
                MATERIAL_MODULES,
                BASE_MATERIAL_MODULES,
                platform_browser_1.BrowserModule,
                forms_1.FormsModule,
                forms_1.ReactiveFormsModule,
                http_1.HttpClientModule,
                animations_1.BrowserAnimationsModule,
                app_routing_module_1.AppRoutingModule,
            ],
            providers: [
                { provide: base_angular_1.ENVIRONMENT, useValue: environment_1.environment },
                { provide: http_1.HTTP_INTERCEPTORS, useClass: base_angular_1.AuthenticationInterceptor, multi: true },
                base_angular_1.DatabaseService,
                base_angular_1.WorkspaceService,
                base_angular_1.AuthenticationService,
                authorization_service_1.AuthorizationService,
            ],
        })
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map