import { Component, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

@Injectable()
export class AppComponent {
  title = 'ClientApp';
  predictions: any;

  constructor(private http: HttpClient) {
    this.getPredictions().subscribe(result => this.predictions = result);
  }

  getPredictions() {
    return this.http.get("api/intrusionDetection");
  }
}
