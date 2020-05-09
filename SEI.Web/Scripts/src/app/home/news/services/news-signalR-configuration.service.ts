import { INewsDataUris } from './news-uris.service';
import { SignalRConfiguration } from 'ng2-signalr';
import { LocalStorageService } from 'ngx-localstorage';
import { Injectable } from '@angular/core';

@Injectable()
export class NewsSignalRConfigurationService {

	public configuration: SignalRConfiguration;

	constructor(
		localStorageService: LocalStorageService
	) {
		const commonUris: INewsDataUris = JSON.parse(localStorageService.get('newsDataUris'));

		const c = new SignalRConfiguration();

		if (commonUris) {
			c.hubName = 'newshub';
			c.qs = {
				// access_token: localStorage.getItem('auth_token')
			};
			c.url = `${commonUris.newsServiceScheme}${commonUris.newsServiceDomain}:${commonUris.newsServicePort}`;
			c.logging = true;
		}

		this.configuration = c;
	}
}
