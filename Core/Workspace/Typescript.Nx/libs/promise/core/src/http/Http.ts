import { HttpResponse } from './HttpResponse';

export interface Http {
  login(url: string, login: string, password: string): Promise<boolean>;

  post(url: string, data: any): Promise<HttpResponse>;
}
