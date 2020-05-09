import { ErrorHandler } from '@angular/core';

export class AmpErrorHandler implements ErrorHandler {
	handleError(error) {
		console.log(error);
	}
}