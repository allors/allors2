var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        Domain.extend(Domain.Person, {
            hello: function () {
                return "Hello " + this.displayName;
            },
            get displayName() {
                if (this.FirstName || this.LastName) {
                    if (this.FirstName && this.LastName) {
                        return this.FirstName + " " + this.LastName;
                    }
                    else if (this.LastName) {
                        return this.LastName;
                    }
                    else {
                        return this.FirstName;
                    }
                }
                if (this.UserName) {
                    return this.UserName;
                }
                return "N/A";
            }
        });
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=Person.js.map