import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './home.component';
import { NewsComponent } from './news/components/news/news.component';
import { NewsConnectionResolver } from './news/components/news/news-connection-resolver';
import { LiveChartComponent } from './live-chart/live-chart.component';

export const homeRoutes: Routes = [
	{
		path: '',
		component: HomeComponent,
		children: [
			{
				path: '',
				children: [
					{
						path: 'chart',
						component: LiveChartComponent
					},
					{
						path: 'news',
						component: NewsComponent,
						resolve: {
							connection: NewsConnectionResolver,
						}
					},
					{ path: '', redirectTo: 'news' }
				]
			}
		]
	}
];

@NgModule({
	imports: [
		RouterModule.forChild(homeRoutes)
	],
	exports: [
		RouterModule
	],
	providers: [
	]
})
export class HomeRoutingModule { }
