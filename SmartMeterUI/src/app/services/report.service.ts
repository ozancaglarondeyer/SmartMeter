import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { GenericResult } from './generic-result';

export interface Report {
  id: string;
  requestedDate: Date;
  status: string;
  content: string;
}

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private apiUrl = `${environment.reportApiUrl}/report`;

  constructor(private http: HttpClient) { }

  getReports(): Observable<GenericResult> {
    return this.http.get<GenericResult>(`${this.apiUrl}/GetReports`);
  }

  getReportDetails(reportId: string): Observable<GenericResult> {
    return this.http.get<GenericResult>(`${this.apiUrl}/${reportId}`);
  }

  createReport(report: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/CreateReport`, report);
  }
}
