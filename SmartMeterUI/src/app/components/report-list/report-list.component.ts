import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CreateReportModalComponent } from '../create-report-modal/create-report-modal.component';
import { ReportService } from '../../services/report.service';
import { ReportDetailComponent } from '../report-detail/report-detail.component';

enum EReportStatus {
  Completed = 1,
  FailedToGenerate = 2,
  InProgress = 3
}

@Component({
  selector: 'app-report-list',
  templateUrl: './report-list.component.html',
  styleUrls: ['./report-list.component.css']
})
export class ReportListComponent implements OnInit {
  reports: any[] = [];

  constructor(private reportService: ReportService, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.fetchReports();
  }

  fetchReports(): void {
    this.reportService.getReports().subscribe((data) => {
      this.reports = data.value;
    });
  }

  openCreateReportModal(): void {
    const dialogRef = this.dialog.open(CreateReportModalComponent, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.fetchReports();
      }
    });
  }

  openReportDetailModal(reportId: string): void {
    const dialogRef = this.dialog.open(ReportDetailComponent, {
      width: '600px',
      data: { reportId: reportId }
    });
  }

  getStatusDescription(status: number): string {
    switch (status) {
      case EReportStatus.Completed:
        return 'Completed';
      case EReportStatus.FailedToGenerate:
        return 'Failed to Generate';
      case EReportStatus.InProgress:
        return 'In Progress';
      default:
        return '';
    }
  }
}
