import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { DataUrisSupplierService } from './../../../common/services/data-uris-supplier.service';
import { INewsDataUris } from './news-uris.service';
import { Observable } from 'rxjs/Observable';
import { LocalStorageService } from 'ngx-localstorage';

@Injectable()
export class NewsUrisResolver implements Resolve<Observable<void>> {

	constructor(
		private uriSupplierService: DataUrisSupplierService,
		private localStorageService: LocalStorageService) { }

	resolve() {
		return this.uriSupplierService.getDataUris<INewsDataUris>('/news/data')
			.map(response => {
				const existingUris = JSON.parse(this.localStorageService.get('newsDataUris'));

				if (!existingUris) {
					this.localStorageService.set('newsDataUris', JSON.stringify(response));
				}
			});
	}

}
