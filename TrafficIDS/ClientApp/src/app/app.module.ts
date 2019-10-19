import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { ChartsModule } from 'ng2-charts';

import { HttpClientModule } from '@angular/common/http';
import { SpikeChartComponent } from './charts/spike-chart/spike-chart.component';
import { ChartService } from './charts/shared/chart-service';
import { FileUploadComponent } from './data-upload/file-upload/file-upload.component';
import { FileUploadService } from './data-upload/shared/file-upload.service';

@NgModule({
	declarations: [
		AppComponent,
		SpikeChartComponent,
		FileUploadComponent
	],
	exports: [
    ],
	imports: [
		BrowserModule,
		AppRoutingModule,
		HttpClientModule,
		ChartsModule
	],
	providers: [
		ChartService,
		FileUploadService
	],
	entryComponents: [
	],
	bootstrap: [
		AppComponent
	]
})
export class AppModule { }
