import { Component } from '@angular/core';
import {
	Router,
	Event as RouterEvent,
	NavigationStart,
	NavigationEnd,
	NavigationCancel,
	NavigationError
} from '@angular/router';
import './operators';

@Component({
	moduleId: module.id,
	selector: 'app-root',
	template: `
        <router-outlet></router-outlet>
        <div class="ui active page dimmer" *ngIf="loading">
		    <div class="content">
			    <div class="center">
	      		    <div class="ui text loader">Loading...</div>
		  	    </div>
	        </div>
	    </div>
      `
})
export class AppComponent {
	public loading: boolean = true;

	constructor(
		private router: Router) {
		router.events.subscribe((event: RouterEvent) => {
			this.navigationInterceptor(event);
		});
	}

	// Shows and hides the loading spinner during RouterEvent changes
	navigationInterceptor(event: RouterEvent): void {
		if (event instanceof NavigationStart) {
			this.loading = true;
		}
		if (event instanceof NavigationEnd) {
			this.loading = false;
		}

		// Set loading state to false in both of the below events to hide the spinner in case a request fails
		if (event instanceof NavigationCancel) {
			this.loading = false;
		}
		if (event instanceof NavigationError) {
			this.loading = false;
		}
	}
}
