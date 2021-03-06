import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class FileUploadService {

	/**
	 * @type {Observable<number>}
	 */
	private progressObservable: Observable<number>;

	/**
	 * @type {number}
	 */
	private progress: number = 0;

	private progressObserver: any;

	/**
	 * Set interval for frequency with which Observable inside Promise will share data with subscribers.
	 *
	 * @param interval
	 */
	private static setUploadUpdateInterval(interval: number): void {
		setInterval(() => {
			console.log();
		}, interval);
	}

	constructor() {
		this.progressObservable = new Observable(observer => {
			this.progressObserver = observer;
		});
	}

	/**
	 * @returns {Observable<number>}
	 */
	public getObserver(): Observable<number> {
		return this.progressObservable;
	}

	/**
	 * Upload files through XMLHttpRequest
	 *
	 * @param url
	 * @param files
	 * @returns {Promise<T>}
	 */
	public upload(url: string, files: File[]): Promise<any> {
		return new Promise((resolve, reject) => {
			const formData: FormData = new FormData(),
				xhr: XMLHttpRequest = new XMLHttpRequest();

			for (let i = 0; i < files.length; i++) {
				formData.append('uploads[]', files[i], files[i].name);
			}

			xhr.onreadystatechange = () => {
				if (xhr.readyState === 4) {
					if (xhr.status === 200) {
						resolve(JSON.parse(xhr.response));
					} else {
						reject(xhr.response);
					}
				}
			};

			FileUploadService.setUploadUpdateInterval(500);

			xhr.upload.onprogress = (event) => {
				this.progress = Math.round(event.loaded / event.total * 100);

				this.progressObserver.next(this.progress);
			};

			xhr.open('POST', url, true);
			xhr.send(formData);
		});
	}
}
