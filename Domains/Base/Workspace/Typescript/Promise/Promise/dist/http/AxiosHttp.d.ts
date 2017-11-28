import { Http } from "./Http";
import { HttpResponse } from "./HttpResponse";
export declare class AxiosHttp implements Http {
    private axiosInstance;
    private token;
    constructor(baseURL: string);
    login(url: string, userName: string, password?: string): Promise<boolean>;
    post(url: string, data: any): Promise<HttpResponse>;
}
