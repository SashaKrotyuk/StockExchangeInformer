import { NewsSignalRConfigurationService } from './services/news-signalR-configuration.service';
import { NewsUrisService, INewsDataUris } from './services/news-uris.service';
import { NewsConnectionResolver } from './components/news/news-connection-resolver';
import { NewsComponent } from './components/news/news.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SignalRConfiguration, SignalRModule } from 'ng2-signalr';

export function createConfig(): SignalRConfiguration {
	return new SignalRConfiguration();
}

@NgModule({
	imports: [
		CommonModule,
		SignalRModule.forRoot(createConfig)
	],
	declarations: [
		NewsComponent
	],
	providers: [
		NewsConnectionResolver,
		NewsUrisService,
		NewsSignalRConfigurationService
	],
	exports: [
		NewsComponent
	]
})
export class NewsModule { }
