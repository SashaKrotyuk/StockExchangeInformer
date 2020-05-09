import { Inject, Injectable, OpaqueToken } from '@angular/core';

// export let TOASTR_TOKEN = new OpaqueToken('toastr');

export class ToastrSettings {
	showDuration = 300;
	maxShown = 3;
	dismiss = 'auto';
	closeButton = true;
	debug = false;
	newestOnTop = true;
	progressBar = true;
	positionClass = 'toast-bottom-right';
	preventDuplicates = false;
	hideDuration = 1000;
	timeOut = 10000;
	extendedTimeOut = 1000;
	showEasing = 'swing';
	hideEasing = 'linear';
	showMethod = 'fadeIn';
	hideMethod = 'fadeOut';
}

@Injectable()
export class ToastrService {

	public toastr;

	constructor(
		@Inject('toastrOptions') public options: any) {
		this.toastr = toastr;
		this.toastr.options = options;
	}

	success(message: string, title?: string) {
		return this.toastr.success(message, title);
	}
	info(message: string, title?: string) {
		return this.toastr.info(message, title);
	}
	warning(message: string, title?: string) {
		return this.toastr.warning(message, title);
	}
	error(message: string, title?: string) {
		return this.toastr.error(message, title);
	}

	clear() {
		this.toastr.clear();
	}
}
