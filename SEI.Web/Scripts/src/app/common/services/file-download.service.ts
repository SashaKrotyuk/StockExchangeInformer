import { Injectable } from '@angular/core';
import { Response } from '@angular/http';

@Injectable()
export class FileDownloadService {
	downloadFile(data: Response, fileName: string) {
		const a = document.createElement('a');
		a.style.display = 'none';
		document.body.appendChild(a);

		const blob = new Blob([data.blob()], { type: 'octet/stream', });
		this.saveAs(blob, fileName);
	}

	saveAs(blobData: Blob, fileName: string): void {
		const windowRef: any = window;

		windowRef.saveAs = windowRef.saveAs || windowRef.webkitSaveAs || windowRef.mozSaveAs || windowRef.msSaveAs ||
			// (msIE) save Blob API
			(!window.navigator.msSaveBlob ? false : (blob: Blob, name: string) => {
				return window.navigator.msSaveBlob(blob, name);
			}) ||
			// save blob via a href and download
			(!window.URL ? false : (blob: Blob, name: string) => {
				// create blobURL
				const blobURL = window.URL.createObjectURL(blob),
					deleteBlobURL = function () {
						setTimeout(function () {
							// delay deleting, otherwise firefox wont download anything
							window.URL.revokeObjectURL(blobURL);
						}, 250);
					};

				// test for download link support
				if ('download' in document.createElement('a')) {
					// create anchor
					const a = document.createElement('a');
					// set attributes
					a.setAttribute('href', blobURL);
					a.setAttribute('download', name);
					// create click event
					a.onclick = deleteBlobURL;

					// append, trigger click event to simulate download, remove
					document.body.appendChild(a);
					a.click();
					document.body.removeChild(a);
				} else {
					// fallback, open resource in new tab
					window.open(blobURL, '_blank', '');
					deleteBlobURL();
				}
			});

		windowRef.saveAs(blobData, fileName);
	}

}
