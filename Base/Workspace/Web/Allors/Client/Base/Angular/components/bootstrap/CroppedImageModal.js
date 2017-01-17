var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var CroppedImageModalTemplate = (function () {
            function CroppedImageModalTemplate() {
            }
            CroppedImageModalTemplate.register = function (templateCache) {
                templateCache.put(CroppedImageModalTemplate.name, CroppedImageModalTemplate.view);
            };
            return CroppedImageModalTemplate;
        }());
        CroppedImageModalTemplate.name = "allors/bootstrap/cropped-image/modal";
        CroppedImageModalTemplate.view = "\n<div class=\"modal-header\">\n    <h3 class=\"modal-title\">Image</h3>\n</div>\n\n<div class=\"modal-body\">\n    \n    <div class=\"row\" style=\"height:20vw;\">\n        <div class=\"col-sm-6\" style=\"height:100%;\">\n            <img-crop   image=\"$ctrl.image\" \n                        area-min-size=\"$ctrl.size\"\n                        result-image=\"$ctrl.croppedImage\" \n                        result-image-size=\"$ctrl.size\"\n                        result-image-format=\"$ctrl.format\"\n                        result-image-quality=\"$ctrl.quality\",\n                        aspect-ratio=\"$ctrl.aspect\">\n            </img-crop>\n        </div>\n\n        <div class=\"col-sm-6 center-block\" style=\"height:100%;\">\n            <img ng-if=\"$ctrl.croppedImage\" ng-src=\"{{$ctrl.croppedImage}}\" class=\"img-responsive img-thumbnail\" style=\"vertical-align: middle; height: 90%\"/>\n        </div>\n    </div>\n\n</div>\n\n<div class=\"modal-footer\">\n    <div class=\"pull-left\">\n        <label class=\"btn btn-default\" for=\"file-selector\">\n            <input id=\"file-selector\" type=\"file\" style=\"display:none;\" model-data-uri=\"$ctrl.image\">\n            Select file\n        </label>\n    </div>\n\n    <button class=\"btn btn-primary\" type=\"button\" ng-click=\"$ctrl.ok()\">OK</button>\n    <button class=\"btn btn-danger\" type=\"button\" ng-click=\"$ctrl.cancel()\">Cancel</button>\n</div>\n";
        Bootstrap.CroppedImageModalTemplate = CroppedImageModalTemplate;
        var CroppedImageModalController = (function () {
            function CroppedImageModalController($scope, $uibModalInstance, $log, $translate, size, format, quality, aspect) {
                this.$scope = $scope;
                this.$uibModalInstance = $uibModalInstance;
                this.size = size;
                this.format = format;
                this.quality = quality;
                this.aspect = aspect;
                this.image = "";
                this.croppedImage = "";
            }
            CroppedImageModalController.prototype.ok = function () {
                this.$uibModalInstance.close(this.croppedImage);
            };
            CroppedImageModalController.prototype.cancel = function () {
                this.$uibModalInstance.dismiss("cancel");
            };
            return CroppedImageModalController;
        }());
        CroppedImageModalController.$inject = ["$scope", "$uibModalInstance", "$log", "$translate", "size", "format", "quality", "aspect"];
        Bootstrap.CroppedImageModalController = CroppedImageModalController;
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=CroppedImageModal.js.map