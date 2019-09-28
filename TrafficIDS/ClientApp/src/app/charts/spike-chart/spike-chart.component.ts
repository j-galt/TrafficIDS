import { Component, OnInit } from '@angular/core';
import { ChartService } from '../shared/chart-service';
import { EvaluatedTraffic } from '../shared/evaluated-traffic.model';

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

	constructor(private chartService: ChartService) {
	}

	ngOnInit(): void {
		this.chartService.getPredictions().subscribe(predictions => {
			this.predictions = predictions

			this.timeStamps = this.predictions.map(p => p.time);
			let values = this.predictions.map(p => p.value);

			let data = {
				data: values,
				label: 'a',
				fill: false
			}

			this.chartData = [data];

			this.chartOptions = {
				scales: {
					xAxes: [{
						display: true
					}],
					yAxes: [{
						display: true
					}],
				}
			};
		});
	}
}
