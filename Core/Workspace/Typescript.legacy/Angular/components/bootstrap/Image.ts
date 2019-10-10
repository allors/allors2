/// <reference path="../../allors.module.ts" />
/// <reference path="Form.ts" />
/// <reference path="internal/Field.ts" />
namespace Allors.Bootstrap {
    export class ImageTemplate {
        static templateName = "allors/bootstrap/image";

        static createDefaultView() {
            return `
<div ng-if="!$ctrl.role">
    <button type="button" class="btn btn-default" ng-click="$ctrl.add()">Add new image</button>
</div>
        
<div ng-if="$ctrl.role.InDataUri">
    <a ng-click="$ctrl.add()">
        <img ng-src="{{$ctrl.role.InDataUri}}" ng-class="$ctrl.imgClass"/>
    </a>
</div>

<div ng-if="!$ctrl.role.InDataUri && $ctrl.role">
    <a ng-click="$ctrl.add()">
        <img ng-src="/media/display/{{$ctrl.role.UniqueId}}?revision={{$ctrl.role.Revision}}" ng-class="$ctrl.imgClass"/>
    </a>
</div>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = ImageTemplate.createDefaultView()) {
            templateCache.put(ImageTemplate.templateName, view);
        }
    }

    export class ImageController extends Field {
        static bindings = {
            object: "<",
            relation: "@",
            imgClass: "@",
            size: "<",
            format: "<",
            quality: "<",
            aspect: "<"
        } as { [binding: string]: string }

        imgClass = "img-responsive";
        size: number;
        format: string;
        quality: number;
        aspect: number;

        static $inject = ["$scope", "$uibModal", "$log", "$translate"];
        constructor(private $scope: angular.IScope, private $uibModal: angular.ui.bootstrap.IModalService, $log: angular.ILogService, $translate: angular.translate.ITranslateService) {
            super($log, $translate);
        }
        
        add() {
            const modalInstance = this.$uibModal.open({
                templateUrl: ImageModalTemplate.templateName,
                controller: ImageModalController,
                controllerAs: "$ctrl",
                resolve: {
                    size: () => this.size,
                    format: () => this.format,
                    quality: () => this.quality,
                    aspect: () => this.aspect
                }
            });

            modalInstance.result
                .then(selectedItem => {
                    if (!this.role) {
                        this.role = this.object.session.create("Media");
                    }

                    var media = this.role as Domain.Media;
                    media.InDataUri = selectedItem;
                })
                .catch(()=>{});;
        }
    }

    angular
        .module("allors")
        .component("bImage", {
            controller: ImageController,
            templateUrl: ImageTemplate.templateName,
            require: FormController.require,
            bindings: ImageController.bindings
        });
}
