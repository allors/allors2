/// <reference path="../allors.module.ts" />
namespace Allors.Filters.Humanize {

    angular.module("allors").
        filter("humanize", () => filter);
    
    export var filter = input => {
        return input
            .replace(/([a-z\d])([A-Z])/g, "$1" + " " + "$2")
            .replace(/([A-Z]+)([A-Z][a-z\d]+)/g, "$1" + " " + "$2");
    };
}
