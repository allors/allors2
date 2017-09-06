import { Headers, Http, RequestOptions, Response } from "@angular/http";
import { Observable, Observer } from "rxjs/Rx";
import { Method } from "../../domain";
import {
  InvokeRequest, InvokeResponse, PullResponse, PushRequest,
  PushResponse, ResponseError, ResponseType, SyncRequest, SyncResponse,
} from "../../domain";

export class Database {
  public options: RequestOptions;

  constructor(private http: Http,
              public url: string,
              private postProcessRequestOptions: (requestOptions: RequestOptions) => RequestOptions = (v: RequestOptions) => v) {
    const headers: any = new Headers(
      {
        "Accept": "application/json",
        "Content-Type": "application/json",
      });
    this.options = new RequestOptions({ headers });
  }

  public pull(name: string, params?: any): Observable<PullResponse> {
    const serviceName: string = this.fullyQualifiedUrl(name + "/Pull");

    return this.http
      .post(serviceName, params, this.postProcessRequestOptions(this.options))
      .map((response: Response) => {
        const pullResponse: PullResponse = response.json();
        pullResponse.responseType = ResponseType.Pull;
        return pullResponse;
      });
  }

  public sync(syncRequest: SyncRequest): Observable<SyncResponse> {

    const serviceName: string = this.fullyQualifiedUrl("Database/Sync");
    return this.http
      .post(serviceName, syncRequest, this.postProcessRequestOptions(this.options))
      .map((response: Response) => {
        const syncResponse: SyncResponse = response.json();
        syncResponse.responseType = ResponseType.Sync;
        return syncResponse;
      });
  }

  public push(pushRequest: PushRequest): Observable<PushResponse> {

    const serviceName: string = this.fullyQualifiedUrl("Database/Push");
    return this.http
      .post(serviceName, pushRequest, this.postProcessRequestOptions(this.options))
      .map((response: Response) => {
        const pushResponse: PushResponse = response.json();
        pushResponse.responseType = ResponseType.Sync;

        if (pushResponse.hasErrors) {
          throw new ResponseError(pushResponse);
        }

        return pushResponse;
      });
  }

  public invoke(method: Method): Observable<InvokeResponse>;
  public invoke(service: string, args?: any): Observable<InvokeResponse>;
  public invoke(methodOrService: Method | string, args?: any): Observable<InvokeResponse> {

    if (methodOrService instanceof Method) {
      return this.invokeMethod(methodOrService);
    } else {
      return this.invokeService(methodOrService, args);
    }
  }

  private invokeMethod(method: Method): Observable<InvokeResponse> {
    const invokeRequest: InvokeRequest = {
      i: method.object.id,
      m: method.name,
      v: method.object.version,
    };

    const serviceName: string = this.fullyQualifiedUrl("Database/Invoke");
    return this.http.post(serviceName, invokeRequest, this.postProcessRequestOptions(this.options))
      .map((response: Response) => {
        const invokeResponse: InvokeResponse = response.json() as InvokeResponse;
        invokeResponse.responseType = ResponseType.Invoke;

        if (invokeResponse.hasErrors) {
          throw new ResponseError(invokeResponse);
        }

        return invokeResponse;
      });
  }

  private invokeService(methodOrService: string, args?: any): Observable<InvokeResponse> {
    const service: string = this.fullyQualifiedUrl(methodOrService + "/Pull");
    return this.http.post(service, args, this.options)
      .map((response: Response) => {
        const invokeResponse: InvokeResponse = response.json();
        invokeResponse.responseType = ResponseType.Invoke;

        if (invokeResponse.hasErrors) {
          throw new ResponseError(invokeResponse);
        }

        return invokeResponse;
      });

  }

  private fullyQualifiedUrl(localUrl: string): string {
    return this.url + localUrl;
  }
}
