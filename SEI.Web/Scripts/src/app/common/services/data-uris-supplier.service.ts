import { Injectable } from '@angular/core';

import { Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { HttpClientService } from './http-client.service';

@Injectable()
export class DataUrisSupplierService {

	constructor(private httpClient: HttpClientService) {
	}

	public getDataUris<T>(dataUri: string): Observable<T> {

		return this.httpClient.get(dataUri)
			.map(r => r.json() as T);
	}

}
