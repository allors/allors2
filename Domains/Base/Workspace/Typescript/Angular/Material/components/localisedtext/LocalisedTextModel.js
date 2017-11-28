"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class LocalisedTextModel {
    constructor(component, locale) {
        this.component = component;
        this.locale = locale;
    }
    get object() {
        const all = this.component.model;
        if (all) {
            const filtered = all.filter((v) => (v.Locale === this.locale));
            return filtered ? filtered[0] : undefined;
        }
    }
    get text() {
        return this.object ? this.object.Text : undefined;
    }
    set text(value) {
        if (!this.object) {
            const localisedText = this.component.object.session.create("LocalisedText");
            localisedText.Locale = this.locale;
            this.component.object.add(this.component.roleType.name, localisedText);
        }
        this.object.Text = value;
    }
    get name() {
        return this.component.name + "_" + this.locale.Name;
    }
    get label() {
        return this.component.label + " (" + this.locale.Name + ")";
    }
}
exports.LocalisedTextModel = LocalisedTextModel;
