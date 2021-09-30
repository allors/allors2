/// <reference path="../../allors.module.ts" />
namespace Allors.Bootstrap {
    export class ImageModalTemplate {
        static templateName = "allors/bootstrap/image/modal";

        private static view =
`
<div class="modal-header">
    <h3 class="modal-title">Image</h3>
</div>

<div class="modal-body">
    
    <div class="row" style="height:20vw;">
        <div class="col-sm-12 center-block" style="height:100%;">
            <img ng-if="$ctrl.image" ng-src="{{$ctrl.image}}" class="img-responsive img-thumbnail" style="vertical-align: middle; height: 90%"/>
        </div>
    </div>

</div>

<div class="modal-footer">
    <div class="pull-left">
        <label class="btn btn-default" for="file-selector">
            <input id="file-selector" type="file" style="display:none;" model-data-uri="$ctrl.image">
            Upload an image
        </label>
    </div>

    <button ng-enabled="$ctrl.image" class="btn btn-primary" type="button" ng-click="$ctrl.ok()">OK</button>
    <button class="btn btn-danger" type="button" ng-click="$ctrl.cancel()">Cancel</button>
</div>
`;
        
        static register(templateCache: angular.ITemplateCacheService) {
            templateCache.put(ImageModalTemplate.templateName, ImageModalTemplate.view);
        }
    }

    export class ImageModalController {

        image = "";
        croppedImage = "";

        static $inject = ["$scope", "$uibModalInstance", "$log", "$translate", "size", "format", "quality", "aspect"];
        constructor(private $scope: angular.IScope, private $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance, $log: angular.ILogService, $translate: angular.translate.ITranslateService, private size: number, private format: string, private quality: number, private aspect: number) {
        }

        ok() {
            this.$uibModalInstance.close(this.image);
        }

        cancel() {
            this.$uibModalInstance.dismiss("cancel");
        }
    }
}
