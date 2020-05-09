import { Component } from '@angular/core';

@Component({
	template: `
	<h2 class="errorMessage">Something went wrong...</h2>
	<div class="ui container navigation">
	  <button class="ui button" routerLink="/">
		Back to site
	  </button>
	</div>
  `,
	styles: [`
	.errorMessage { 
	  margin-top:150px;
	  text-align: center; 
	}

	.navigation {
	  text-align: center;
	}
	`]
})
export class ErrorComponent {

	constructor() { }

}
