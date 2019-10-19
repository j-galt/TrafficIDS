import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class FileUploadService {

	serverGeneratedFileName: string = '';

	constructor(private readonly http: HttpClient) { }

	postFile(fileToUpload: File): Observable<string> {
		const formData: FormData = new FormData();

		formData.append('fileKey', fileToUpload, fileToUpload.name);

		return this.http
			.post('api/intrusionDetection', formData)
			.pipe(map((result: any) => {
				this.serverGeneratedFileName = result.fileName; return result;
			}));
	}

	getFileName(): string {
		return this.serverGeneratedFileName;
	}
}