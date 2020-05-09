import { NewsModule } from './news/news.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpModule, Http } from '@angular/http';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { HomeComponent } from './home.component';
import { HomeRoutingModule } from './home-routing.module';
import { HomeNavbarComponent } from './components/home-navbar/home-navbar.component';
import { LiveChartComponent } from './live-chart/live-chart.component';

@NgModule({
	imports: [
		CommonModule,
		NewsModule,
		FormsModule,
		HomeRoutingModule
	],
	exports: [],
	declarations: [
		HomeComponent,
		HomeNavbarComponent,
		LiveChartComponent
	],
	providers: [
	]
})
export class HomeModule { }
