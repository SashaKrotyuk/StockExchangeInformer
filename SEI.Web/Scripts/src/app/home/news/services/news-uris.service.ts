import { Injectable } from '@angular/core';
import { LocalStorageService } from 'ngx-localstorage';

export interface INewsDataUris {
	newsServiceScheme: string;
	newsServiceDomain: string;
	newsServicePort: string;
}

@Injectable()
export class NewsUrisService {

	private _uris: INewsDataUris;

	constructor(
		private localStorageService: LocalStorageService) {
	}

	get uris(): INewsDataUris {
		if (!this._uris) {
			this._uris = JSON.parse(this.localStorageService.get('newsDataUris'));
		}

		return this._uris;
	}

}
