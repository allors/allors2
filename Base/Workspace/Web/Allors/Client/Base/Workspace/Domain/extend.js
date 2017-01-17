var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        function extend(type, extension) {
            Object.getOwnPropertyNames(extension).forEach(function (name) {
                var ownPropertyDescriptor = Object.getOwnPropertyDescriptor(extension, name);
                Object.defineProperty(type.prototype, name, ownPropertyDescriptor);
            });
        }
        Domain.extend = extend;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=extend.js.map