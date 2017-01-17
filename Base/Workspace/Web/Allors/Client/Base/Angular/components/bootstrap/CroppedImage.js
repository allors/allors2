var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var CroppedImageTemplate = (function () {
            function CroppedImageTemplate() {
            }
            CroppedImageTemplate.createDefaultView = function () {
                return "\n<div ng-if=\"!$ctrl.role\">\n    <button type=\"button\" class=\"btn btn-default\" ng-click=\"$ctrl.add()\">Add new image</button>\n</div>\n        \n<div ng-if=\"$ctrl.role.InDataUri\">\n    <a ng-click=\"$ctrl.add()\">\n        <img ng-src=\"{{$ctrl.role.InDataUri}}\" ng-class=\"$ctrl.imgClass\"/>\n    </a>\n</div>\n<div ng-if=\"!$ctrl.role.InDataUri && $ctrl.role\">\n    <a ng-click=\"$ctrl.add()\">\n        <img ng-src=\"/media/display/{{$ctrl.role.UniqueId}}?revision={{$ctrl.role.Revision}}\" ng-class=\"$ctrl.imgClass\"/>\n    </a>\n</div>\n";
            };
            CroppedImageTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = CroppedImageTemplate.createDefaultView(); }
                templateCache.put(CroppedImageTemplate.name, view);
            };
            return CroppedImageTemplate;
        }());
        CroppedImageTemplate.name = "allors/bootstrap/cropped-image";
        Bootstrap.CroppedImageTemplate = CroppedImageTemplate;
        var CroppedImageController = (function (_super) {
            __extends(CroppedImageController, _super);
            function CroppedImageController($scope, $uibModal, $log, $translate) {
                var _this = _super.call(this, $log, $translate) || this;
                _this.$scope = $scope;
                _this.$uibModal = $uibModal;
                _this.imgClass = "img-responsive";
                return _this;
            }
            CroppedImageController.prototype.add = function () {
                var _this = this;
                var modalInstance = this.$uibModal.open({
                    templateUrl: Bootstrap.CroppedImageModalTemplate.name,
                    controller: Bootstrap.CroppedImageModalController,
                    controllerAs: "$ctrl",
                    resolve: {
                        size: function () { return _this.size; },
                        format: function () { return _this.format; },
                        quality: function () { return _this.quality; },
                        aspect: function () { return _this.aspect; }
                    }
                });
                modalInstance.result.then(function (selectedItem) {
                    if (!_this.role) {
                        _this.role = _this.object.session.create("Media");
                    }
                    var media = _this.role;
                    media.InDataUri = selectedItem;
                });
            };
            return CroppedImageController;
        }(Bootstrap.Field));
        CroppedImageController.bindings = {
            object: "<",
            relation: "@",
            imgClass: "@",
            size: "<",
            format: "<",
            quality: "<",
            aspect: "<"
        };
        CroppedImageController.$inject = ["$scope", "$uibModal", "$log", "$translate"];
        Bootstrap.CroppedImageController = CroppedImageController;
        angular
            .module("allors")
            .component("bCroppedImage", {
            controller: CroppedImageController,
            templateUrl: CroppedImageTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: CroppedImageController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=CroppedImage.js.map