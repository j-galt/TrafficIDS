import { Component, OnInit } from '@angular/core';
import { FileUploadService } from '../shared/file-upload.service';

@Component({
	selector: 'file-upload',
	templateUrl: './file-upload.component.html',
	styleUrls: ['./file-upload.component.css']
})

export class FileUploadComponent implements OnInit {

	fileToUpload: File = null;
	enabled: boolean = false;

	constructor(
		private readonly fileUploadService: FileUploadService) { }

	ngOnInit() {
	}

	handleFileInput(files: FileList) {
		this.fileToUpload = files.item(0);
		this.uploadFileToActivity();
	}

	uploadFileToActivity() {
		this.fileUploadService.postFile(this.fileToUpload).subscribe(data => {
			this.enabled = true;
		}, error => {
			console.log(error);
		});
	}
}
