import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { Error404Component } from './common/components/404/404.component';
import { ErrorComponent } from './common/components/error/error.component';

import { DataUrisSupplierService } from './common/services/data-uris-supplier.service';
import { NewsUrisResolver } from 'app/home/news/services/news-uris-resolver';
// import { HomeUrisResolverService } from './home/services/home-uris-resolver.service';

export const appRoutes: Routes = [

	{
		path: '',
		loadChildren: './home/home.module#HomeModule',
		resolve: {
			newsUris: NewsUrisResolver
		},
		data: { preload: true }
	},
	// {
	// 	path: 'admin',
	// 	loadChildren: 'app/admin/admin.module#AdminModule'
	// },
	{ path: 'error', component: ErrorComponent },
	{ path: '404', component: Error404Component }
];

@NgModule({
	imports: [
		RouterModule.forRoot(appRoutes)
	],
	exports: [
		RouterModule
	],
	providers: [
		DataUrisSupplierService,
		NewsUrisResolver
	]
})
export class AppRoutingModule { }
