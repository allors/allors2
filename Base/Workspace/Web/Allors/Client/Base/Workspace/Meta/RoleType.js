var Allors;
(function (Allors) {
    var Meta;
    (function (Meta) {
        var RoleType = (function () {
            function RoleType(roleTypeData) {
                this.name = roleTypeData.name;
                this.objectType = roleTypeData.objectType;
                this.isUnit = roleTypeData.isUnit;
                this.isOne = roleTypeData.isOne;
                this.isRequired = roleTypeData.isRequired;
            }
            Object.defineProperty(RoleType.prototype, "isComposite", {
                get: function () { return !this.isUnit; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(RoleType.prototype, "isMany", {
                get: function () { return this.isComposite && !this.isOne; },
                enumerable: true,
                configurable: true
            });
            return RoleType;
        }());
        Meta.RoleType = RoleType;
    })(Meta = Allors.Meta || (Allors.Meta = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=RoleType.js.map