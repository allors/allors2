"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const PostalAddress_g_1 = require("../generated/PostalAddress.g");
Object.defineProperty(PostalAddress_g_1.PostalAddress.prototype, "displayName", {
    get() {
        let fullAddress;
        if (this.Address1 || this.Address2 || this.Address3) {
            if (this.Address1) {
                fullAddress = this.Address1;
            }
            else if (this.Address2) {
                fullAddress = this.Address2;
            }
            else {
                fullAddress = this.Address3;
            }
        }
        if (fullAddress === "" && this.PostalBoundary.PostalCode) {
            fullAddress += this.PostalBoundary.PostalCode;
        }
        else {
            fullAddress += " " + this.PostalBoundary.PostalCode;
        }
        if (fullAddress === "" && this.PostalBoundary.Locality) {
            fullAddress += this.PostalBoundary.Locality;
        }
        else {
            fullAddress += " " + this.PostalBoundary.Locality;
        }
        if (fullAddress === "" && this.PostalBoundary.Country.Name) {
            return fullAddress += this.PostalBoundary.Country.Name;
        }
        else {
            return fullAddress += " " + this.PostalBoundary.Country.Name;
        }
    },
});
