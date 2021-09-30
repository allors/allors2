import axios from 'axios';
import { AxiosInstance, AxiosRequestConfig } from 'axios';
// import * as http from "http";
// import * as https from "http";

import { AuthenticationResponse } from './AuthenticationResponse';
import { Http } from './Http';
import { HttpResponse } from './HttpResponse';

export class AxiosHttp implements Http {
  public axios: AxiosInstance;
  public token: string;

  constructor(config: AxiosRequestConfig) {
    this.axios = axios.create(config);
  }

  public login(url: string, userName: string, password?: string): Promise<boolean> {
    return new Promise((resolve, reject) => {
      const data = { Username: userName, Password: password };
      this.axios
        .post(url, data)
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

  public get(url: string, params?: any): Promise<HttpResponse> {
    return new Promise((resolve, reject) => {
      const config: AxiosRequestConfig = {
        headers: { Authorization: `Bearer ${this.token}` },
        params,
      };

      this.axios
        .get(url, config)
        .then((v) => {
          const httpResponse = { data: v.data };
          resolve(httpResponse);
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

      this.axios
        .post(url, data, config)
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
