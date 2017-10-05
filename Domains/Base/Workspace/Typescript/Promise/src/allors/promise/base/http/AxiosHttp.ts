import axios from "axios";
import { AxiosInstance, AxiosRequestConfig } from "axios";
import * as http from "http";
import * as https from "http";
import { AuthenticationResponse } from "./AuthenticationResponse";
import { Http } from "./Http";
import { HttpResponse } from "./HttpResponse";

export class AxiosHttp implements Http {
    private axiosInstance: AxiosInstance;
    private token: string;

    constructor(baseURL: string) {
        this.axiosInstance = axios.create({
            baseURL,
            httpAgent: new http.Agent({ keepAlive: true }),
            httpsAgent: new https.Agent({ keepAlive: true }),
            maxContentLength: 50 * 1000 * 1000,
            maxRedirects: 10,
            timeout: 60000,
        });
    }

    public login(url: string, userName: string, password: string = null): Promise<boolean> {
        return new Promise((resolve, reject) => {
            const data = { Username: userName, Password: password };
            this.axiosInstance.post(url, data)
                .then((v) => {

                    const result: AuthenticationResponse = v.data;
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

    public post(url: string, data: any): Promise<HttpResponse> {

        return new Promise((resolve, reject) => {

            const config: AxiosRequestConfig = {
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
