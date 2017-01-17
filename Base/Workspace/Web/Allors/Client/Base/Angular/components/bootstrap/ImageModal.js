var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var ImageModalTemplate = (function () {
            function ImageModalTemplate() {
            }
            ImageModalTemplate.register = function (templateCache) {
                templateCache.put(ImageModalTemplate.name, ImageModalTemplate.view);
            };
            return ImageModalTemplate;
        }());
        ImageModalTemplate.name = "allors/bootstrap/image/modal";
        ImageModalTemplate.view = "\n<div class=\"modal-header\">\n    <h3 class=\"modal-title\">Image</h3>\n</div>\n\n<div class=\"modal-body\">\n    \n    <div class=\"row\" style=\"height:20vw;\">\n        <div class=\"col-sm-12 center-block\" style=\"height:100%;\">\n            <img ng-if=\"$ctrl.image\" ng-src=\"{{$ctrl.image}}\" class=\"img-responsive img-thumbnail\" style=\"vertical-align: middle; height: 90%\"/>\n        </div>\n    </div>\n\n</div>\n\n<div class=\"modal-footer\">\n    <div class=\"pull-left\">\n        <label class=\"btn btn-default\" for=\"file-selector\">\n            <input id=\"file-selector\" type=\"file\" style=\"display:none;\" model-data-uri=\"$ctrl.image\">\n            Upload an image\n        </label>\n    </div>\n\n    <button ng-enabled=\"$ctrl.image\" class=\"btn btn-primary\" type=\"button\" ng-click=\"$ctrl.ok()\">OK</button>\n    <button class=\"btn btn-danger\" type=\"button\" ng-click=\"$ctrl.cancel()\">Cancel</button>\n</div>\n";
        Bootstrap.ImageModalTemplate = ImageModalTemplate;
        var ImageModalController = (function () {
            function ImageModalController($scope, $uibModalInstance, $log, $translate, size, format, quality, aspect) {
                this.$scope = $scope;
                this.$uibModalInstance = $uibModalInstance;
                this.size = size;
                this.format = format;
                this.quality = quality;
                this.aspect = aspect;
                this.image = "";
                this.croppedImage = "";
            }
            ImageModalController.prototype.ok = function () {
                this.$uibModalInstance.close(this.image);
            };
            ImageModalController.prototype.cancel = function () {
                this.$uibModalInstance.dismiss("cancel");
            };
            return ImageModalController;
        }());
        ImageModalController.$inject = ["$scope", "$uibModalInstance", "$log", "$translate", "size", "format", "quality", "aspect"];
        Bootstrap.ImageModalController = ImageModalController;
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=ImageModal.js.map