import { NgxLocalStorageModule } from 'ngx-localstorage';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Http, HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { DataService } from './common/services/data.service';
import { HttpClientService } from './common/services/http-client.service';
import { ToastrSettings, ToastrService } from './common/services/toastr.service';
import { RouterModule } from '@angular/router';
import { ErrorComponent } from './common/components/error/error.component';
import { Error404Component } from './common/components/404/404.component';
import { AppRoutingModule } from './app-routing.module';

@NgModule({
	declarations: [
		AppComponent,
		Error404Component,
		ErrorComponent
	],
	imports: [
		BrowserModule,
		FormsModule,
		HttpModule,
		// AppTranslateModule,
		// TranslateModule.forRoot({
		// 	loader: {
		// 		provide: TranslateLoader,
		// 		useFactory: createTranslateLoader,
		// 		deps: [Http, 'localizationPrefix', 'localizationSuffix']
		// 	}
		// }),
		NgxLocalStorageModule.forRoot({
			prefix: 'sei'
		}),
		AppRoutingModule
	],
	exports: [
		RouterModule
	],
	providers: [
		DataService,
		HttpClientService,
		{ provide: 'toastrOptions', useClass: ToastrSettings },
		ToastrService,
		// { provide: 'localizationPrefix', useValue: '/assets/i18n/' },
		// { provide: 'localizationSuffix', useValue: '.json' },
	],
	bootstrap: [AppComponent]
})
export class AppModule { }
