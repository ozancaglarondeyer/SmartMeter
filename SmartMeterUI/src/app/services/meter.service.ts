import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { GenericResult } from './generic-result';


@Injectable({
  providedIn: 'root'
})
export class MeterService {
  private apiUrl = `${environment.meterApiUrl}/meter`;

  constructor(private http: HttpClient) { }

  getMeters(parameters: any): Observable<GenericResult> {
    let params = new HttpParams();
    if (parameters.serialNumber) {
      params = params.append('SerialNumber', parameters.serialNumber);
    }
    if (parameters.lastReadingStartDate) {
      params = params.append('LastReadingStartDate', parameters.lastReadingStartDate);
    }
    if (parameters.lastReadingEndDate) {
      params = params.append('LastReadingEndDate', parameters.lastReadingEndDate);
    }

    return this.http.get<GenericResult>(`${this.apiUrl}/GetMeters`, { params });
  }



  getMeter(id: number): Observable<GenericResult> {
    return this.http.get<GenericResult>(`${this.apiUrl}/${id}`);
  }

  addMeter(meter: any): Observable<any> {
    return this.http.post<any>(this.apiUrl + "/AddMeterReading", meter);
  }

  getMeterSelection(): Observable<GenericResult> {
    return this.http.get<GenericResult>(`${this.apiUrl}/GetMeterSelection`);
  }
}
