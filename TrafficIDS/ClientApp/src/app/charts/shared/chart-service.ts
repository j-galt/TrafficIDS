import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { EvaluatedTraffic } from './evaluated-traffic.model';

@Injectable()
export class ChartService {

	constructor(private http: HttpClient) { }

	getPredictions(): Observable<any> {
		return this.http.get<EvaluatedTraffic>("api/intrusionDetection")
			.pipe(map(res => res));
	}
}