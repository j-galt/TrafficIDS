import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { EvaluatedTraffic } from './evaluated-traffic.model';

@Injectable()
export class ChartService {

	constructor(private http: HttpClient) { }

	getPredictions(fileName: string): Observable<any> {
		let params = new HttpParams().append("fileName", fileName);

		return this.http.get<EvaluatedTraffic>("api/intrusionDetection", { params })
			.pipe(map(res => res));
	}
}