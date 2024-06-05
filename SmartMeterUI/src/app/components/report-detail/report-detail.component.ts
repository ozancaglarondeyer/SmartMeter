import { Component, Inject } from '@angular/core';
import { ReportService } from '../../services/report.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-report-detail',
  templateUrl: './report-detail.component.html',
  styleUrl: './report-detail.component.css'
})
export class ReportDetailComponent {
  reportDetails: any[] = [];

  constructor(
    private reportService: ReportService,
    public dialogRef: MatDialogRef<ReportDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { reportId: string }
  ) { }

  ngOnInit(): void {
    this.fetchReportDetails();
  }

  fetchReportDetails(): void {
    this.reportService.getReportDetails(this.data.reportId).subscribe((details) => {
      this.reportDetails = details.value;
    });
  }

  close(): void {
    this.dialogRef.close();
  }
}
