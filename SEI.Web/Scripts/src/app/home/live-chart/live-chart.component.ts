import { AfterViewInit, Component, OnInit } from '@angular/core';

declare var TradingView: any;

@Component({
	moduleId: module.id,
	selector: 'sei-live-chart',
	templateUrl: './live-chart.component.html',
	styleUrls: ['./live-chart.component.css']
})
export class LiveChartComponent implements OnInit, AfterViewInit {

	constructor() { }

	ngOnInit() {
	}

	ngAfterViewInit() {
		new TradingView.widget({
			'container_id': 'chartWidget',
			'width': '100%',
			'height': '610',
			'symbol': 'NASDAQ:AAPL',
			'interval': 'D',
			'timezone': 'Etc/UTC',
			'theme': 'White',
			'style': '2',
			'locale': 'en',
			'toolbar_bg': '#f1f3f6',
			'enable_publishing': false,
			'withdateranges': true,
			'hide_side_toolbar': false,
			'allow_symbol_change': true,
			'details': true,
			'hotlist': true,
			'calendar': true,
			'news': [
				'stocktwits',
				'headlines'
			]
		});
	}

}
