import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MeterService } from '../../services/meter.service';
import { ReportService } from '../../services/report.service';

@Component({
  selector: 'app-create-report-modal',
  templateUrl: './create-report-modal.component.html',
  styleUrls: ['./create-report-modal.component.css']
})
export class CreateReportModalComponent implements OnInit {
  createReportForm: FormGroup;
  meters: any[] = [];

  constructor(
    private fb: FormBuilder,
    private meterService: MeterService,
    private reportService: ReportService,
    public dialogRef: MatDialogRef<CreateReportModalComponent>
  ) {
    this.createReportForm = this.fb.group({
      meterId: ['', Validators.required],
      name: ['', Validators.required],
      serialNumber: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.fetchMeters();
  }

  fetchMeters(): void {
    this.meterService.getMeterSelection().subscribe((data) => {
      this.meters = data.value;
    });
  }

  onMeterChange(event: any): void {
    const selectedMeter = this.meters.find(m => m.id === event.value);
    if (selectedMeter) {
      this.createReportForm.patchValue({ serialNumber: selectedMeter.serialNumber });
    }
  }

  onSubmit(): void {
    if (this.createReportForm.valid) {
      const reportData = this.createReportForm.value;
      this.reportService.createReport(reportData).subscribe(() => {
        this.dialogRef.close(true);
      });
    }
  }

  close(): void {
    this.dialogRef.close(false);
  }
}
