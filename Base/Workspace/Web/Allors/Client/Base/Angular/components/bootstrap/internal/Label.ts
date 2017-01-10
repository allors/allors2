namespace Allors.Bootstrap {
    export class LabelTemplate {
        static name = "allors/bootstrap/label";

        private static view = 
`
<label class="control-label" ng-class="$ctrl.form.horizontal ? 'col-sm-2' : '' ">{{$ctrl.field.label}}
    <span ng-if="$ctrl.field.help" class="fa fa-question-circle"
          uib-tooltip="{{$ctrl.field.help}}"
          tooltip-placement="right">
    </span>
</label>
`;

        static register(templateCache: angular.ITemplateCacheService) {
            templateCache.put(LabelTemplate.name, LabelTemplate.view);
        }
    }

    class LabelComponent {
        field: Field;

        static $inject = ["$log"];
        constructor($log: angular.ILogService) {
        }
    }

    angular
        .module("allors")
        .component("bLabel", {
            controller: LabelComponent,
            templateUrl: LabelTemplate.name,
            require: {
                form: "^bForm"
            },
            bindings: {
                field: "<"
            }
        });
}
