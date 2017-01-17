var Allors;
(function (Allors) {
    var Meta;
    (function (Meta) {
        var ObjectType = (function () {
            function ObjectType(data) {
                var _this = this;
                this.roleTypeByName = {};
                this.name = data.name;
                _.forEach(data.roleTypes, function (roleTypeData) {
                    var roleType = new Meta.RoleType(roleTypeData);
                    _this.roleTypeByName[roleType.name] = roleType;
                });
            }
            return ObjectType;
        }());
        Meta.ObjectType = ObjectType;
    })(Meta = Allors.Meta || (Allors.Meta = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=ObjectType.js.map