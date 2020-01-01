import { Component, OnInit } from '@angular/core';
import { ChartService } from '../shared/chart-service';
import { EvaluatedTraffic } from '../shared/evaluated-traffic.model';
import { FileUploadService } from 'src/app/data-upload/shared/file-upload.service';

@Component({
	selector: 'spike-chart',
	templateUrl: './spike-chart.component.html',
	styleUrls: ['./spike-chart.component.css']
})

export class SpikeChartComponent implements OnInit {

	predictions: EvaluatedTraffic[];
	chart: Chart;

	chartOptions: object;
	timeStamps: string[];
	chartLegend = true;
	chartData: object[];

	constructor(
		private readonly chartService: ChartService,
		private readonly fileUploadService: FileUploadService) {
	}

	ngOnInit(): void {
		let name = this.fileUploadService.getFileName();

		this.chartService.getPredictions(name).subscribe(predictions => {
			this.predictions = predictions

			this.timeStamps = this.predictions.map(p => new Date(p.time).toLocaleString());
			let values = this.predictions.map(p => p.value);
			
			var opt = this.fillPointColors(predictions);

			let data = {
				data: values,
				label: "Packets statistic",
				fill: true,
				pointBackgroundColor: opt.pointBackgroundColors,
				pointRadius: opt.pointRadiuses,
				pointHoverBackgroundColor: opt.pointBackgroundColors,
				tension: 0.5,
				borderColor: 'rgba(52,152,219,0.5)',
				backgroundColor: 'rgba(214,234,248,0.4)',
			}

			this.chartData = [data];

			this.chartOptions = {
				tooltips: {
					titleFontSize: 14,
					bodyFontSize: 12,
					label: 'lol',
					callbacks: {
					}
				}
			}
		});
	}

	fillPointColors(evaluatedTraffic: EvaluatedTraffic[]): any {
		let pointBackgroundColors: string[] = [];
		let pointRadiuses: number[] = [];

		for (let i = 0; i < evaluatedTraffic.length; i++) {
			if (evaluatedTraffic[i].prediction[0] === 1) {
				pointBackgroundColors.push("#f58368");
				pointRadiuses.push(7)
			} else {
				pointBackgroundColors.push("#ABB2B9");
				pointRadiuses.push(5)
			}
		}

		return {pointBackgroundColors, pointRadiuses};
	}
}
