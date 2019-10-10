/// <reference path="allors.module.ts" />
namespace Allors {
    export class Events {
        private static refreshEventName = "allors.refresh";

        constructor(public context: Context, public $rootScope: angular.IRootScopeService, public scope: angular.IScope) {
        }

        on(eventName: string, handler: () => void) {
            this.scope.$on(eventName, handler);
        }

        onRefresh(handler: () => void) {
            this.on(Events.refreshEventName, handler);
        }

        broadcast(eventName: string) {
            this.$rootScope.$broadcast(eventName, this.context.name);
        }

        broadcastRefresh() {
            this.broadcast(Events.refreshEventName);
        }
    }
}