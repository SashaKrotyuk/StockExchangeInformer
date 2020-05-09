import { Component, AfterViewInit } from '@angular/core';

@Component({
	moduleId: module.id,
	selector: 'sei-home',
	template: `
        <sei-home-navbar></sei-home-navbar>
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
export class HomeComponent implements AfterViewInit {

	public loading: boolean = true;

	constructor() { }

	ngAfterViewInit() {
		this.loading = false;
	}

}
