import { NewsSignalRConfigurationService } from './../../services/news-signalR-configuration.service';
import { Resolve, Router } from '@angular/router';
import { SignalR, SignalRConnection } from 'ng2-signalr';
import { Injectable } from '@angular/core';

@Injectable()
export class NewsConnectionResolver implements Resolve<SignalRConnection> {

	constructor(
		public _signalR: SignalR,
		public router: Router,
		private signaRConfigurationService: NewsSignalRConfigurationService) { }

	resolve() {
		return this._signalR.connect(this.signaRConfigurationService.configuration);
	}

}
