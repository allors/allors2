/// <reference path="../../../allors.module.ts" />
namespace Allors.Bootstrap {
    export abstract class Field {
        label;
        placeholder;
        help;
      
        form: FormController;
        object: SessionObject;
        relation: string;
        display: string;
        lookup: (any) => angular.IPromise<any>;
        
        constructor(public $log: angular.ILogService, public $translate: angular.translate.ITranslateService) {
        }

        get objectType(): Meta.ObjectType {
            try {
                return this.object && this.object.objectType;
            } catch (e) {
                return undefined;
            }
        }

        get roleType(): Meta.RoleType {
            try {
                const objectType = this.object.objectType;
                return objectType.roleTypeByName[this.relation];
            } catch (e) {
                return undefined;
            }
        }

        get canRead(): boolean {
            try {
                let canRead = false;
                if (this.object) {
                    canRead = this.object.canRead(this.relation);
                }

                return canRead;
            } catch (e) {
                return undefined;
            }
        }

        get canWrite(): boolean {
            try {
                let canWrite = false;
                if (this.object) {
                    canWrite = this.object.canWrite(this.relation);
                }

                return canWrite;
            } catch (e) {
                return undefined;
            }
        }
        
        get role(): any {
            try {
                return this.object && this.object[this.relation];
            } catch(e) {
                return undefined;
            }
        }

        set role(value: any) {
            try {
                this.object[this.relation] = value;
            } catch (e) {
            }
        }

        get displayValue(): any {
            try {
                return this.role && this.role[this.display];
            } catch (e) {
                return undefined;
            }
        }

        $onInit() {
            this.derive();
        }

        $onChanges() {
            this.derive();
        };

        derive() {
            try {
                if (this.roleType && this.$translate) {
                    if (this.label === undefined) {
                        this.label = null;

                        const key1 = `meta_${this.objectType.name}_${this.roleType.name}_Label`;
                        const key2 = `meta_${this.roleType.objectType}_${this.roleType.name}_Label`;
                        this.translate(key1, key2, (value) => this.label = value);

                        if (this.label === undefined || this.label === null) {
                            this.label = Filters.Humanize.filter(this.relation);

                            const suffix = "Enum";
                            if (this.label.indexOf(suffix, this.label.length - suffix.length) !== -1) {
                                this.label = this.label.substring(0, this.label.length - suffix.length);
                            }
                        }
                    }

                    if (this.placeholder === undefined) {
                        this.placeholder = null;

                        const key1 = `meta_${this.objectType.name}_${this.roleType.name}_Placeholder`;
                        const key2 = `meta_${this.roleType.objectType}_${this.roleType.name}_Placeholder`;
                        this.translate(key1, key2, (value) => this.placeholder = value);
                    }

                    if (this.help === undefined) {
                        this.help = null;

                        const key1 = `meta_${this.objectType.name}_${this.roleType.name}_Help`;
                        const key2 = `meta_${this.roleType.objectType}_${this.roleType.name}_Help`;
                        this.translate(key1, key2, (value) => this.help = value);
                    }
                }
            } catch (e) {
                this.$log.error("Could not translate field");
            }
        }

        translate(key1: string, key2: string, set: (translation: string) => void, setDefault?: () => void) {
            this.$translate(key1)
                .then(translation => {
                    if (key1 !== translation) {
                        set(translation);
                    } else {
                        this.$translate(key2)
                            .then(translation => {
                                if (key2 !== translation) {
                                    set(translation);
                                } else {
                                    if (setDefault) {
                                        setDefault();
                                    }
                                }
                            })
                            .catch(()=>{});;
                    }
                })
                .catch(()=>{});;

        }
    }
}
