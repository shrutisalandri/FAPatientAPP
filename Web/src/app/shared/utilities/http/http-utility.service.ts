import { ResponseContentType } from '@angular/http';
import { Config } from '../config';
import { GlobalEventsManager } from '../global-events-manager';
import { HttpClient, HttpHeaders, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/finally';
import 'rxjs/add/operator/concatAll';
import 'rxjs/add/operator/shareReplay';

@Injectable()
export class HttpUtility {

    private baseApiUrl: string;
    private baseLocalUrl: string;

    constructor(
        private config: Config,
        private http: HttpClient,
        private router: Router,
        private _globalEventsManager: GlobalEventsManager) {
        this.baseLocalUrl = this.config.getByKey('baseLocalUrl');
        this.baseApiUrl = this.config.getByKey('baseApiUrl');
    }

    /**
     * Post
     * @param url
     * @param body
     * @param options
     */
    public post(url: string, body: any, options?: HttpParams) {
        return this.http.post(this.getApiUrl(url), body)
            .catch((e) => this.handleError(e));
    }

    /**
     * Put
     * @param url
     * @param body
     * @param options
     */
    public put(url: string, body: any, options?: HttpParams) {
        return this.http.put(this.getApiUrl(url), body)
            .catch((e) => this.handleError(e));
    }
    
    /**
     * delete
     * @param url
     * @param options
     */
    public delete(url: string, options?: HttpParams) {
        return this.http.delete(this.getApiUrl(url))
            .catch((e) => this.handleError(e));
    }
    /**
     * get
     * @param url
     * @param options
     */
    public get(url: string, options?: HttpParams) {
        return this.http.get(this.getApiUrl(url))
            .shareReplay()
            .catch((e) => this.handleError(e));

    }

    public getFileSource(url: string, options?: HttpParams) {
        return this.http.get(url = this.getApiUrl(url),
            { responseType: 'arraybuffer' }
        )
            // .map((response: any) => {
            //     return response.arrayBuffer();
            // })
            // .map((arrayBuffer: ArrayBuffer) => new Uint8Array(arrayBuffer))
            .shareReplay()
            .catch((e) => this.handleError(e));

        // return this.http.get(this.getApiUrl('Document/GetFileResult/1'),
        //     { responseType: 'arraybuffer' }
        // )
        //     .shareReplay()
        //     .catch((e) => this.handleError(e));
    }

    /**
     * getFile
     * @param url
     * @param options
     */
    public getFile(url: string, options?: HttpParams) {
        window.open(this.getApiUrl(url));
    }

    /**
     * handleError
     * @param response
     */
    private handleError(errorResponse: any) {
        // in a real world app, we may send the server to some remote logging infrastructure
        // instead of just logging it to the console
        let errorStatus = '404'
        console.log("error occured.", errorResponse);
        if (errorResponse.url.search('AppBackend/Login') === -1) {
            if (errorResponse instanceof HttpErrorResponse) {
                errorStatus = errorResponse.status > 0 ? errorResponse.status.toLocaleString() : '404';
            }
            if (errorStatus !== '401') {
                this._globalEventsManager.error(errorStatus);

                // Redirect the user after login
                return this.router.navigate(['/error']);
            }
        }
        return Observable.throw(errorResponse)
    }


    /**
     * getApiUrl
     * @param url
     */
    private getApiUrl(url) {
        if (url.indexOf("localhost") >= 0) {
            return this.baseLocalUrl + url.replace('localhost/', '');
        } else {
            return this.baseApiUrl + url;
        }
    }

}
