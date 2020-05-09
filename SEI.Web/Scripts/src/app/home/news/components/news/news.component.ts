import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SignalRConnection } from 'ng2-signalr';
import * as _ from 'lodash';
import * as moment from 'moment';

interface INewsItem {
	title: string;
	description: string;
	date: Date;
	timestamp: string;
	url: string;
	imageUrl: string;
	sourceName: string;
	uniqueIdentifier: string;
}

declare var TradingView: any;

@Component({
	moduleId: module.id,
	selector: 'sei-news',
	templateUrl: './news.component.html',
	styleUrls: ['./news.component.css']
})
export class NewsComponent implements OnInit, AfterViewInit, OnDestroy {

	public connection: SignalRConnection;
	public newsList: INewsItem[] = [];

	constructor(
		public router: Router,
		public route: ActivatedRoute
	) { }

	ngOnInit() {
		this.connection = this.route.snapshot.data['connection'];
		if (!this.connection) {
			this.router.navigate(['/']);
		} else {
			const onLatestNews = this.connection.listenFor<INewsItem[]>('onLatestNews');
			onLatestNews.subscribe(news => {
				const newsList = _.concat(this.newsList, news);
				const sortedByDateNews = _.sortBy(newsList, item => moment(item.date).toDate());
				this.newsList = _.reverse(sortedByDateNews);
			});

			this.connection.invoke('getLatestNews');
		}
	}

	ngAfterViewInit() {
		new TradingView.IdeasStreamWidget({
			'container_id': 'ideasWidget',
			'startingCount': 2,
			'width': '100%',
			'height': 400,
			'mode': 'integrate',
			'bgColor': '#f2f5f8',
			'headerColor': '#4FAC00',
			'borderColor': '#dce1e6',
			'locale': 'en',
			'sort': 'trending',
			'time': 'day',
			'interval': 'all',
			'stream': 'all'
		});

		// new TradingView.ChatWidgetEmbed({
		// 	'container_id': 'tradersChatWidget',
		// 	'room': 'general',
		// 	'width': 'auto',
		// 	'height': '400px',
		// 	'locale': 'en'
		// });
	}

	getDate(date) {
		return moment(date).format('LLLL');
	}

	ngOnDestroy() {

	}

}
