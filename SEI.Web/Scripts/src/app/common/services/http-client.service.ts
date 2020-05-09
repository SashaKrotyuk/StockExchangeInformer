import { Injectable } from '@angular/core';
import { Headers, Http, Response, ResponseContentType, ResponseType } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { LocalStorageService } from 'ngx-localstorage';

export interface IHttpClientService {
	createAuthorizationHeader(headers: Headers): void;
	get(url: string): Observable<Response>;
	post(url: string, data: any): Observable<Response>;
}

@Injectable()
export class HttpClientService {

	constructor(
		private http: Http,
		private localStorageService: LocalStorageService) { }

	createAuthorizationHeader(headers: Headers): void {
		headers.append('Authorization', 'Bearer ' +
			this.localStorageService.get('auth_token'));
	}

	public get(url: string, responseType?: ResponseContentType, headers?: Headers): Observable<Response> {
		if (!headers) {
			headers = new Headers();
		}

		headers.append('Content-Type', 'application/json');
		headers.append('Accept', 'application/json');

		this.createAuthorizationHeader(headers);
		return this.http.get(url, {
			headers: headers,
			responseType: responseType
		});
	}

	public post(url: string, data: any, responseType?: ResponseContentType, headers?: Headers): Observable<Response> {
		if (!headers) {
			headers = new Headers();
			headers.append('Content-Type', 'application/json');
			headers.append('Accept', 'application/json');
		}

		this.createAuthorizationHeader(headers);
		return this.http.post(url, data, {
			headers: headers,
			responseType: responseType
		});
	}

	public put(url: string, data: any): Observable<Response> {
		const headers = new Headers();
		headers.append('Content-Type', 'application/json');
		headers.append('Accept', 'application/json');

		this.createAuthorizationHeader(headers);
		return this.http.put(url, data, {
			headers: headers
		});
	}

	public delete(url: string, body?: any): Observable<Response> {
		const headers = new Headers();
		headers.append('Content-Type', 'application/json');
		headers.append('Accept', 'application/json');

		this.createAuthorizationHeader(headers);
		return this.http.delete(url, {
			headers: headers,
			body: body
		});
	}

}
