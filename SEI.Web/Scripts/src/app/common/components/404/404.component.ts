import { Component } from '@angular/core';

@Component({
	template: `
    <h1 class="errorMessage">404'd</h1>
    <div class="ui container navigation">
      <button class="ui button" routerLink="/">
        Back to site
      </button>
    </div>
  `,
	styles: [`
    .errorMessage { 
      margin-top:150px; 
      font-size: 170px;
      text-align: center; 
    }
    
    .navigation {
      text-align: center;
    }
    `]
})
export class Error404Component {

	constructor() { }

}
