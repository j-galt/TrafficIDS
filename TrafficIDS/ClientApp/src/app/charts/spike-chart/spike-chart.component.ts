import { Component, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'spike-chart',
  templateUrl: './spike-chart.component.html',
  styleUrls: ['./spike-chart.component.css']
})

@Injectable()
export class SpikeChartComponent {
  predictions: any;

  constructor(private http: HttpClient) {
    this.getPredictions().subscribe(result => this.predictions = result);
  }

  getPredictions() {
    return this.http.get("api/intrusionDetection");
  }
}
