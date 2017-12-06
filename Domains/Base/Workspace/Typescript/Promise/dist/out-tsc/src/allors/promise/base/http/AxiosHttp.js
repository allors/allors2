"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const axios_1 = require("axios");
class AxiosHttp {
    constructor(baseURL) {
        this.axiosInstance = axios_1.default.create({
            baseURL,
            // httpAgent: new http.Agent({ keepAlive: true }),
            // httpsAgent: new https.Agent({ keepAlive: true }),
            maxContentLength: 50 * 1000 * 1000,
            maxRedirects: 10,
            timeout: 60000,
        });
    }
    login(url, userName, password = null) {
        return new Promise((resolve, reject) => {
            const data = { Username: userName, Password: password };
            this.axiosInstance.post(url, data)
                .then((v) => {
                const result = v.data;
                if (result.authenticated) {
                    this.token = result.token;
                }
                resolve(result.authenticated);
            })
                .catch((e) => {
                reject(e);
            });
        });
    }
    post(url, data) {
        return new Promise((resolve, reject) => {
            const config = {
                headers: { Authorization: `Bearer ${this.token}` },
            };
            this.axiosInstance.post(url, data, config)
                .then((v) => {
                const httpResponse = { data: v.data };
                resolve(httpResponse);
            })
                .catch((e) => {
                reject(e);
            });
        });
    }
}
exports.AxiosHttp = AxiosHttp;
//# sourceMappingURL=AxiosHttp.js.map