/// <reference path="../allors.module.ts" />
namespace Allors {

    class ContentEditable implements ng.IDirective {
        restrict = "A";
        require = "?ngModel";

        constructor(private $sce: ng.ISCEService) {
        }

        link(scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ngModel: any) {
            if (!ngModel) return;

            ngModel.$render = () => {
                element.html(this.$sce.getTrustedHtml(ngModel.$viewValue) || "");
            };

            element.on("blur keyup change",
                () => {
                    scope.$evalAsync(read);
                });
            
            function read() {
                let html = $.trim(element.html()).replace(/&nbsp;/g, "\u00a0");
                if (html === "" || html === "<br>") {
                    html = null;
                }

                ngModel.$setViewValue(html);
            }
        }

        static factory(): ng.IDirectiveFactory {
            const directive = ($sce: ng.ISCEService) => new ContentEditable($sce);
            directive.$inject = ["$sce"];
            return directive;
        }
    }

    angular.module("allors").directive("contenteditable", ContentEditable.factory());
}
